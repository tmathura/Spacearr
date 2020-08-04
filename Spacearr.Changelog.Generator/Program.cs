using Autofac;
using Spacearr.Common.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Spacearr.Changelog.Generator
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length != 5)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Too many arguments supplied, supply only the 'GitHub Owner', 'GitHub Repo', 'Directory Of Clone', 'Commit Sha', 'GitHub Token'");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (args.Any(string.IsNullOrWhiteSpace))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("One of the arguments is blank");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                var builder = new ContainerBuilder();
                AutofacConfig.Configure(args[0], args[1], args[2], args[3], args[4], builder);
                var container = builder.Build();
                var changelogGeneratorService = container.Resolve<IChangelogGeneratorService>();
                await changelogGeneratorService.CreateChangelog();
            }
        }
    }
}
