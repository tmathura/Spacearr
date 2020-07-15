using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Spacearr.Common.Tests.Factories
{
    public static class OctokitRelease
    {
        public static List<Release> CreateOctokitReleases(int total)
        {
            var releases = new List<Release>();

            for (var i = 0; i < total; i++)
            {
                var releaseAsserts = new List<ReleaseAsset>
                {
                    new ReleaseAsset("url", 1, "1", "apk", "label", "state", "contentType",
                        1, 1, DateTimeOffset.Now, DateTimeOffset.Now, $"browserDownloadUrl_apk_{i}", new Author()),
                    new ReleaseAsset("url", 1, "1", "windowsservice", "label", "state", "contentType",
                        1, 1, DateTimeOffset.Now, DateTimeOffset.Now, $"browserDownloadUrl_windowsservice_{i}", new Author()),
                    new ReleaseAsset("url", 1, "1", "workerservice", "label", "state", "contentType",
                        1, 1, DateTimeOffset.Now, DateTimeOffset.Now, $"browserDownloadUrl_workerservice_{i}", new Author()),
                    new ReleaseAsset("url", 1, "1", "uwp", "label", "state", "contentType",
                        1, 1, DateTimeOffset.Now, DateTimeOffset.Now, $"browserDownloadUrl_uwp_{i}", new Author())
                };

                releases.Add(new Release("url", "htmlUrl", "assetsUrl", "uploadUrl", 1, "nodeId", $"1.0.0.{i}", 
                    "targetCommitish", "name", "body", false, false, DateTime.Now, DateTime.Now, new Author(), "tarballUrl", 
                    "zipballUrl", releaseAsserts));
            }

            return releases.OrderByDescending(x => x.CreatedAt).ToList();
        }
    }
}