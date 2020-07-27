using Autofac;
using Octokit;
using Spacearr.Common.Services.Implementations;
using Spacearr.Common.Services.Interfaces;

namespace Spacearr.ChangelogGenerator
{
    public class AutofacConfig
    {
        public static void Configure(ContainerBuilder builder)
        {
            builder.Register(c => new GitHubClient(new ProductHeaderValue("Spacearr")) { Credentials = new Credentials("0c0edc25d3fef06a7cd3e71bb9d2b2eb92b30021") }).As<IGitHubClient>().SingleInstance();
            builder.RegisterType<ChangelogGeneratorService>().As<IChangelogGeneratorService>().SingleInstance();
        }
    }
}