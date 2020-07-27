using Autofac;
using Spacearr.Common.Services.Interfaces;
using System.Threading.Tasks;

namespace Spacearr.ChangelogGenerator
{
    public class Program
    {
        public static async Task Main()
        {
            var builder = new ContainerBuilder();
            AutofacConfig.Configure(builder);
            var container = builder.Build();
            var changelogGeneratorService = container.Resolve<IChangelogGeneratorService>();
            await changelogGeneratorService.CreateChangelog();
        }
    }
}
