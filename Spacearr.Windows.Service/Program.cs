using Autofac;
using Spacearr.Common.Interfaces;
using Spacearr.Pusher.API.Interfaces;
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
                    new Service(container.Resolve<IPusher>(), container.Resolve<INotificationTimer>())
                };
                ServiceBase.Run(servicesToRun);
            }
            catch (Exception ex)
            {
                using (var eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Spacearr Windows Service";
                    eventLog.WriteEntry($"{ex.Message}", EventLogEntryType.Error);
                }
            }
        }
    }
}