using Microsoft.Toolkit.Parsers.Markdown;
using Microsoft.Toolkit.Parsers.Markdown.Blocks;
using Octokit;
using Spacearr.Common.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly IGitHubClient _gitHubClient;

        public ChangelogGeneratorService(string owner, string repositoryName, string repoDirectory, IGitHubClient gitHubClient)
        {
            _owner = owner;
            _repositoryName = repositoryName;
            _repoDirectory = repoDirectory;
            _gitHubClient = gitHubClient;
        }

        /// <summary>
        /// Create changelog.
        /// </summary>
        /// <returns></returns>
        public async Task CreateChangelog()
        {
            var process = new Process
            {
                StartInfo =
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    WorkingDirectory = _repoDirectory,
                    FileName = "cmd.exe",
                    Arguments = @"/C git branch --show-current",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true
                }
            };
            process.Start();
            var output = string.Empty;
            while (!process.HasExited)
            {
                output += await process.StandardOutput.ReadToEndAsync();
            }

            var currentBranch = output.Replace("\n", string.Empty);
            var mergeToBranch = currentBranch == "master" ? "dev" : "master";

            var changelogPath = Path.Combine(_repoDirectory, "CHANGELOG.md");

            var changelogDocument = new MarkdownDocument();
            var oldChangelog = string.Empty;
            if (File.Exists(changelogPath))
            {
                var changelogText = File.ReadAllText(changelogPath);
                changelogDocument.Parse(changelogText);

                if (changelogDocument.Blocks.Any(x => x.ToString().Contains("Unreleased Changes")))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Getting previous CHANGELOG.md without unreleased changes (Changelog path: {changelogPath})");
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
                    Console.WriteLine("Got CHANGELOG.md without unreleased changes");
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
            Console.WriteLine($"Writing new release info to CHANGELOG.md (Changelog path: {changelogPath})");
            File.WriteAllLines(changelogPath, new[] { releaseText });

            if (!string.IsNullOrWhiteSpace(oldChangelog))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Writing old release info to CHANGELOG.md (Changelog path: {changelogPath})");
                File.AppendAllLines(changelogPath, new[] { oldChangelog });
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Finished writing to CHANGELOG.md");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Starting git add for CHANGELOG.md");
            process.StartInfo.Arguments = @"/C git add CHANGELOG.md";
            process.Start();
            while (!process.HasExited) { }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Finished git add for CHANGELOG.md");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Starting git commit for CHANGELOG.md");
            process.StartInfo.Arguments = @"/C git commit -m ""Update CHANGELOG.md""";
            process.Start();
            while (!process.HasExited) { }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Finished git commit to CHANGELOG.md");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Starting git push");
            process.StartInfo.Arguments = @"/C git push";
            process.Start();
            while (!process.HasExited) { }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Finished git push");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Starting git switch (Branch: {mergeToBranch})");
            process.StartInfo.Arguments = $@"/C git checkout {mergeToBranch}";
            process.Start();
            while (!process.HasExited) { }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Finished git switch");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Starting git merge (Branch: {currentBranch})");
            process.StartInfo.Arguments = $@"/C git merge {currentBranch}";
            process.Start();
            while (!process.HasExited) { }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Finished git merge");

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
            Console.WriteLine($"Getting commits from GitHub (Owner: {_owner}, Repository: {_repositoryName}, Branch: {branchName}, Since: {since.DateTime.ToLongDateString()})");
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
