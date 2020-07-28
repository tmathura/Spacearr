using Autofac;
using Octokit;
using Spacearr.Common.Services.Implementations;
using Spacearr.Common.Services.Interfaces;

namespace Spacearr.Changelog.Generator
{
    public class AutofacConfig
    {
        public static void Configure(string owner, string repositoryName, string repoDirectory, string currentBranch, string gitHubToken, ContainerBuilder builder)
        {
            builder.Register(c => new GitHubClient(new ProductHeaderValue("Spacearr")) { Credentials = new Credentials(gitHubToken) }).As<IGitHubClient>().SingleInstance();
            builder.Register(c => new ChangelogGeneratorService(owner, repositoryName, repoDirectory, currentBranch, c.Resolve<IGitHubClient>())).As<IChangelogGeneratorService>().SingleInstance();
        }
    }
}