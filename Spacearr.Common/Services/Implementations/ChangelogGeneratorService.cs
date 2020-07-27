using Octokit;
using Spacearr.Common.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Toolkit.Parsers.Markdown;
using Microsoft.Toolkit.Parsers.Markdown.Blocks;

namespace Spacearr.Common.Services.Implementations
{
    public class ChangelogGeneratorService : IChangelogGeneratorService
    {
        private const string RepoName = "Spacearr";
        private const string Owner = "tmathura";
        private readonly IGitHubClient _gitHubClient;

        public ChangelogGeneratorService(IGitHubClient gitHubClient)
        {
            _gitHubClient = gitHubClient;
        }

        /// <summary>
        /// Create changelog.
        /// </summary>
        /// <returns></returns>
        public async Task CreateChangelog()
        {
            var changelogPath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent?.Parent?.Parent?.Parent?.FullName ?? string.Empty, "CHANGELOG.md");

            var changelogDocument = new MarkdownDocument();
            var oldChangelog = string.Empty;
            if (File.Exists(changelogPath))
            {
                var changelogText = File.ReadAllText(changelogPath);
                changelogDocument.Parse(changelogText);

                if (changelogDocument.Blocks.Any(x => x.ToString().Contains("Unreleased Changes")))
                {
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
                }
            }

            var releases = await _gitHubClient.Repository.Release.GetAll(Owner, RepoName);
            var latestRelease = releases[0];
            var previousReleaseDate = DateTimeOffset.Now;
            if (releases.Count > 0)
            {
                previousReleaseDate = releases[1].CreatedAt;
            }

            var latestCommit = await _gitHubClient.Repository.Commit.Get(Owner, RepoName, latestRelease.TargetCommitish);
            var masterCommits = await GetCommits("master", previousReleaseDate);
            var devCommits = await GetCommits("dev", previousReleaseDate);

            var diffCommits = GetCommitsDiff(devCommits, masterCommits);

            var formattedDiffCommits = FormatCommits(diffCommits);
            var formattedMasterCommits = FormatCommits(masterCommits);

            var issues = await GetReleaseDetails(IssueTypeQualifier.Issue, $"{Owner}/{RepoName}");
            var pulls = await GetReleaseDetails(IssueTypeQualifier.PullRequest, $"{Owner}/{RepoName}");

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

            File.WriteAllLines(changelogPath, new[] { releaseText });

            if (!string.IsNullOrWhiteSpace(oldChangelog))
            {
                File.AppendAllLines(changelogPath, new[] { oldChangelog });
            }

            var newChangelogText = File.ReadAllText(changelogPath);
            var updateChangeSet = await _gitHubClient.Repository.Content.UpdateFile(Owner, RepoName, @"C:\projects\spacearr\CHANGELOG.md", 
                new UpdateFileRequest("Update CHANGELOG.md", newChangelogText, devCommits[0].Sha, "dev"));
        }

        /// <summary>
        /// Get commits from a specified date until now.
        /// </summary>
        /// <param name="branchName"></param>
        /// <param name="since"></param>
        /// <returns></returns>
        private async Task<IReadOnlyList<GitHubCommit>> GetCommits(string branchName, DateTimeOffset since)
        {
            return await _gitHubClient.Repository.Commit.GetAll(Owner, RepoName, new CommitRequest { Sha = branchName, Since = since, Until = DateTimeOffset.Now });
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
