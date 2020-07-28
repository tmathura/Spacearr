using Autofac;
using Spacearr.Common.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Spacearr.ChangelogGenerator
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Too many arguments supplied, supply only the 'GitHub Owner', 'GitHub Repo', 'Directory Of Clone', 'GitHub Token'");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                var builder = new ContainerBuilder();
                AutofacConfig.Configure(args[0], args[1], args[2], args[3], builder);
                var container = builder.Build();
                var changelogGeneratorService = container.Resolve<IChangelogGeneratorService>();
                await changelogGeneratorService.CreateChangelog();
            }
        }
    }
}
