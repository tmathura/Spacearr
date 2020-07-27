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
            builder.Register(c => new GitHubClient(new ProductHeaderValue("Spacearr")) { Credentials = new Credentials("") }).As<IGitHubClient>().SingleInstance();
            builder.RegisterType<ChangelogGeneratorService>().As<IChangelogGeneratorService>().SingleInstance();
        }
    }
}