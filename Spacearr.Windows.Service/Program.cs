using Autofac;
using Microsoft.Extensions.Configuration;
using Spacearr.Common.Timers.Interfaces;
using Spacearr.Pusher.API;
using System;
using System.Diagnostics;
using System.ServiceProcess;

namespace Spacearr.Windows.Service
{
    internal static class Program
    {
        private static void Main()
        {
            try
            {
                var builder = new ContainerBuilder();
                AutofacConfig.Configure(builder);
                var container = builder.Build();

                var servicesToRun = new ServiceBase[]
                {
                    new Service(container.Resolve<IConfiguration>(), container.Resolve<IPusher>(), container.Resolve<ILowSpaceTimer>())
                };
                ServiceBase.Run(servicesToRun);
            }
            catch (Exception ex)
            {
                using (var eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Spacearr Windows Service";
                    eventLog.WriteEntry($"{ex}", EventLogEntryType.Error);
                }
            }
        }
    }
}