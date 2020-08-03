using Microsoft.Toolkit.Parsers.Markdown;
using Microsoft.Toolkit.Parsers.Markdown.Blocks;
using Octokit;
using Spacearr.Common.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Spacearr.Common.Services.Implementations
{
    public class ChangelogGeneratorService : IChangelogGeneratorService
    {
        private readonly string _owner;
        private readonly string _repositoryName;
        private readonly string _repoDirectory;
        private readonly string _currentBranch;
        private readonly IGitHubClient _gitHubClient;

        public ChangelogGeneratorService(string owner, string repositoryName, string repoDirectory, string currentBranch, IGitHubClient gitHubClient)
        {
            _owner = owner;
            _repositoryName = repositoryName;
            _repoDirectory = repoDirectory;
            _currentBranch = currentBranch;
            _gitHubClient = gitHubClient;
        }

        /// <summary>
        /// Create changelog.
        /// </summary>
        /// <returns></returns>
        public async Task CreateChangelog()
        {
            var mergeToBranch = _currentBranch.ToLower() == "master" ? "dev" : "master";
            
            const string changelogFileName = "CHANGELOG.md";
            var changelogPath = Path.Combine(_repoDirectory, changelogFileName);

            var changelogDocument = new MarkdownDocument();
            var oldChangelog = string.Empty;
            if (File.Exists(changelogPath))
            {
                var changelogText = File.ReadAllText(changelogPath);
                changelogDocument.Parse(changelogText);

                if (changelogDocument.Blocks.Any(x => x.ToString().Contains("Unreleased Changes")))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Getting previous {changelogFileName} without unreleased changes (Changelog path: {changelogPath})");
                    var count = 0;
                    foreach (var element in changelogDocument.Blocks)
                    {
                        if (count !=0 && count != 1)
                        {
                            if (element is HeaderBlock header)
                            {
                                if (header.ToString().Contains("Release "))
                                {
                                    oldChangelog += $"\n\n#{header}";
                                }
                                else
                                {
                                    oldChangelog += $"\n\n##{header}";
                                }
                            }
                            else if (element is ListBlock listItem)
                            {
                                foreach (var listItemBlock in listItem.Items)
                                {
                                    oldChangelog += $"\n - {listItemBlock.Blocks[0]}";
                                }
                            }
                        }
                        count++;
                    }
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Got {changelogFileName} without unreleased changes");
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Getting releases from GitHub (Owner: {_owner}, Repository: {_repositoryName})");
            var releases = await _gitHubClient.Repository.Release.GetAll(_owner, _repositoryName);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Succeeded in getting releases from GitHub");

            var latestRelease = releases[0];
            var previousReleaseDate = DateTimeOffset.Now;
            if (releases.Count > 0)
            {
                previousReleaseDate = releases[1].CreatedAt;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Getting latest commit from GitHub (Owner: {_owner}, Repository: {_repositoryName}, Commit: {latestRelease.TargetCommitish})");
            var latestCommit = await _gitHubClient.Repository.Commit.Get(_owner, _repositoryName, latestRelease.TargetCommitish);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Succeeded in getting latest commit from GitHub");

            var masterCommits = await GetCommits("master", previousReleaseDate);
            var devCommits = await GetCommits("dev", previousReleaseDate);
            var diffCommits = GetCommitsDiff(devCommits, masterCommits);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Formatting commits");
            var formattedDiffCommits = FormatCommits(diffCommits);
            var formattedMasterCommits = FormatCommits(masterCommits);

            var issues = await GetReleaseDetails(IssueTypeQualifier.Issue, $"{_owner}/{_repositoryName}");
            var pulls = await GetReleaseDetails(IssueTypeQualifier.PullRequest, $"{_owner}/{_repositoryName}");

            var releaseText = string.Empty;

            if (!string.IsNullOrWhiteSpace(formattedDiffCommits))
            {
                releaseText += $"# Unreleased Changes{formattedDiffCommits}";
            }

            if (string.IsNullOrWhiteSpace(releaseText))
            {
                releaseText += $"# Release {latestRelease.Name}";
            }
            else
            {
                releaseText += $"\n\n# Release {latestRelease.Name}";
            }

            var latestReleaseBody = !string.IsNullOrWhiteSpace(latestRelease.Body) ? latestRelease.Body : formattedMasterCommits ?? latestCommit.Commit.Message;
            if (!string.IsNullOrWhiteSpace(latestReleaseBody))
            {
                releaseText += $"\n\n## Included Changes{latestReleaseBody}";
            }

            if (!string.IsNullOrWhiteSpace(issues))
            {
                releaseText += $"\n\n## Issues Closed{issues}";
            }

            if (!string.IsNullOrWhiteSpace(pulls))
            {
                releaseText += $"\n\n## Changes Merged{pulls}";
            }
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Writing new release info to {changelogFileName} (Changelog path: {changelogPath})");
            File.WriteAllLines(changelogPath, new[] { releaseText });

            if (!string.IsNullOrWhiteSpace(oldChangelog))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Writing old release info to {changelogFileName} (Changelog path: {changelogPath})");
                File.AppendAllLines(changelogPath, new[] { oldChangelog });
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Finished writing to {changelogFileName}");

            var latestChangeLogText = File.ReadAllText(changelogPath);
            RepositoryContentChangeSet changeSet;
            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Starting upload of CHANGELOG.md to {_currentBranch}");
                var existingFile = await _gitHubClient.Repository.Content.GetAllContentsByRef(_owner, _repositoryName, changelogFileName, _currentBranch);
                changeSet = await _gitHubClient.Repository.Content.UpdateFile(_owner, _repositoryName, changelogFileName,
                    new UpdateFileRequest($"Update {changelogFileName}. {DateTime.Now}", latestChangeLogText + DateTime.UtcNow, existingFile.First().Sha, _currentBranch));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Finished upload of CHANGELOG.md");
            }
            catch (NotFoundException)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Starting upload of CHANGELOG.md to {_currentBranch}");
                changeSet = await _gitHubClient.Repository.Content.CreateFile(_owner, _repositoryName, changelogFileName,
                    new CreateFileRequest($"Create {changelogFileName}. {DateTime.Now}", latestChangeLogText + DateTime.UtcNow, _currentBranch));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Finished upload of CHANGELOG.md");
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Merging CHANGELOG.md to {mergeToBranch}");
            var merge = await _gitHubClient.Repository.Merging.Create(_owner, _repositoryName, new NewMerge(mergeToBranch, changeSet.Commit.Sha) { CommitMessage = $"Merge update {changelogFileName}. {DateTime.Now}" });
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Finished merge of CHANGELOG.md");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Updating reference of {_currentBranch} branch");
            await _gitHubClient.Git.Reference.Update(_owner, _repositoryName, $"heads/{_currentBranch}", new ReferenceUpdate(merge.Sha, true));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Finished updating reference");

            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Get commits from a specified date until now.
        /// </summary>
        /// <param name="branchName"></param>
        /// <param name="since"></param>
        /// <returns></returns>
        private async Task<IReadOnlyList<GitHubCommit>> GetCommits(string branchName, DateTimeOffset since)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Getting commits from GitHub (Owner: {_owner}, Repository: {_repositoryName}, Branch: {branchName}, Since: {since.DateTime})");
            var commits =  await _gitHubClient.Repository.Commit.GetAll(_owner, _repositoryName, new CommitRequest { Sha = branchName, Since = since, Until = DateTimeOffset.Now });
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Succeeded in getting commits from GitHub");
            return commits;
        }

        /// <summary>
        /// Get commits that are not are not in the master branch.
        /// </summary>
        /// <param name="masterCommits"></param>
        /// <param name="devCommits"></param>
        /// <returns></returns>
        private static IEnumerable<GitHubCommit> GetCommitsDiff(IReadOnlyList<GitHubCommit> devCommits, IReadOnlyList<GitHubCommit> masterCommits)
        {
            var commits = new List<GitHubCommit>();

            foreach (var commit in devCommits)
            {
                if (masterCommits.All(x => x.Sha != commit.Sha))
                {
                    commits.Add(commit);
                }
            }
            return commits;
        }

        /// <summary>
        /// Get commits formatted.
        /// </summary>
        /// <param name="commitsList"></param>
        /// <returns></returns>
        private static string FormatCommits(IEnumerable<GitHubCommit> commitsList)
        {
            string formatCommits = null;

            foreach (var commit in commitsList)
            {
                formatCommits += $"\n - [{commit.Commit.Message.Replace("\n\n", "; ").Replace("\n", "; ")}]({commit.Commit.Url})";
            }

            return formatCommits;
        }

        /// <summary>
        /// Get release details.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="repoName"></param>
        /// <returns></returns>
        private async Task<string> GetReleaseDetails(IssueTypeQualifier type, string repoName)
        {
            var twoWeeks = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(14));
            var range = new DateRange(twoWeeks, SearchQualifierOperator.GreaterThanOrEqualTo);
            var request = new SearchIssuesRequest();

            request.Repos.Add(repoName);
            request.Type = type;

            if (type == IssueTypeQualifier.Issue)
            {
                request.Closed = range;
            }
            else
            {
                request.Merged = range;
            }

            var issues = await _gitHubClient.Search.SearchIssues(request);

            var searchResults = string.Empty;
            foreach (var x in issues.Items)
            {
                searchResults += $"\n - [{x.Title}]({x.HtmlUrl})";
            }

            return searchResults;
        }
    }
}
