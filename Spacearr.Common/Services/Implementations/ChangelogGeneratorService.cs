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
        private readonly string _repositoryOwner;
        private readonly string _repositoryName;
        private readonly string _repoDirectory;
        private readonly string _commitSha;
        private readonly IGitHubClient _gitHubClient;

        public ChangelogGeneratorService(string repositoryOwner, string repositoryName, string repoDirectory, string commitSha, IGitHubClient gitHubClient)
        {
            _repositoryOwner = repositoryOwner;
            _repositoryName = repositoryName;
            _repoDirectory = repoDirectory;
            _commitSha = commitSha;
            _gitHubClient = gitHubClient;
        }

        /// <summary>
        /// Create changelog.
        /// </summary>
        /// <returns></returns>
        public async Task CreateChangelog()
        {
            const string masterBranchName = "master";
            const string devBranchName = "dev";
            const string changelogFileName = "CHANGELOG.md";

            var changelogPath = Path.Combine(_repoDirectory, changelogFileName);

            var changelogDocument = new MarkdownDocument();
            var oldChangelog = string.Empty;
            if (File.Exists(changelogPath))
            {
                var changelogText = File.ReadAllText(changelogPath);
                changelogDocument.Parse(changelogText);

                var count = 0;
                if (!changelogDocument.Blocks.Any(x => x.ToString().Contains("Unreleased Changes")))
                {
                    count = -1;
                }
                foreach (var element in changelogDocument.Blocks)
                {
                    if (count !=0 && count != 1)
                    {
                        switch (element)
                        {
                            case HeaderBlock header when header.ToString().Contains("Release "):
                                oldChangelog += $"\n\n#{header}";
                                break;
                            case HeaderBlock header:
                                oldChangelog += $"\n\n##{header}";
                                break;
                            case ListBlock listItem:
                            {
                                foreach (var listItemBlock in listItem.Items)
                                {
                                    oldChangelog += $"\n - {listItemBlock.Blocks[0]}";
                                }

                                break;
                            }
                        }
                    }

                    if (changelogDocument.Blocks.Any(x => x.ToString().Contains("Unreleased Changes")))
                    {
                        count++;
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Getting releases from GitHub (Owner: {_repositoryOwner}, Repository: {_repositoryName})");
            var releases = await _gitHubClient.Repository.Release.GetAll(_repositoryOwner, _repositoryName);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Succeeded in getting releases from GitHub");

            var latestRelease = releases[0];
            var previousReleaseDate = DateTimeOffset.Now;
            if (releases.Count > 0)
            {
                previousReleaseDate = releases[1].CreatedAt;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Getting latest commit from GitHub (Owner: {_repositoryOwner}, Repository: {_repositoryName}, Commit: {latestRelease.TargetCommitish})");
            var latestCommit = await _gitHubClient.Repository.Commit.Get(_repositoryOwner, _repositoryName, latestRelease.TargetCommitish);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Succeeded in getting latest commit from GitHub");

            var masterCommits = await GetCommits(masterBranchName, previousReleaseDate);
            var devCommits = await GetCommits(devBranchName, previousReleaseDate);
            var diffCommits = GetCommitsDiff(devCommits, masterCommits);

            var commitShaDate = masterCommits.FirstOrDefault(x => x.Sha == _commitSha)?.Commit.Committer.Date;

            IReadOnlyList<GitHubCommit> currentMasterCommits = masterCommits.Where(commit => commit.Commit.Committer.Date <= commitShaDate).ToList();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Formatting commits");
            var formattedDiffCommits = FormatCommits(diffCommits);
            var formattedMasterCommits = FormatCommits(currentMasterCommits.Count > 0 ? currentMasterCommits : masterCommits);

            var issues = await GetReleaseDetails(IssueTypeQualifier.Issue, previousReleaseDate);
            var pulls = await GetReleaseDetails(IssueTypeQualifier.PullRequest, previousReleaseDate);

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

            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Starting upload of CHANGELOG.md to {masterBranchName}");
                var existingFile = await _gitHubClient.Repository.Content.GetAllContentsByRef(_repositoryOwner, _repositoryName, changelogFileName, masterBranchName);
                await _gitHubClient.Repository.Content.UpdateFile(_repositoryOwner, _repositoryName, changelogFileName,
                    new UpdateFileRequest($"Update {changelogFileName}. {DateTime.Now}", latestChangeLogText + DateTime.UtcNow, existingFile.First().Sha, masterBranchName));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Finished upload of CHANGELOG.md");
            }
            catch (NotFoundException)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Starting upload of CHANGELOG.md to {masterBranchName}");
                await _gitHubClient.Repository.Content.CreateFile(_repositoryOwner, _repositoryName, changelogFileName,
                    new CreateFileRequest($"Create {changelogFileName}. {DateTime.Now}", latestChangeLogText + DateTime.UtcNow, masterBranchName));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Finished upload of CHANGELOG.md");
            }

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
            Console.WriteLine($"Getting commits from GitHub (Owner: {_repositoryOwner}, Repository: {_repositoryName}, Branch: {branchName}, Since: {since.DateTime})");
            var commits =  await _gitHubClient.Repository.Commit.GetAll(_repositoryOwner, _repositoryName, new CommitRequest { Sha = branchName, Since = since, Until = DateTimeOffset.Now });
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
                formatCommits += $"\n - [{commit.Commit.Message.Replace("\n\n", "; ").Replace("\n", "; ")}]({commit.HtmlUrl}) ([{commit.Commit.Author.Name}]({commit.Author.HtmlUrl}))";
            }

            return formatCommits;
        }

        /// <summary>
        /// Get release details.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="since"></param>
        /// <returns></returns>
        private async Task<string> GetReleaseDetails(IssueTypeQualifier type, DateTimeOffset since)
        {
            var range = new DateRange(since, SearchQualifierOperator.GreaterThanOrEqualTo);
            var request = new SearchIssuesRequest();

            request.Repos.Add($"{_repositoryOwner}/{_repositoryName}");
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
