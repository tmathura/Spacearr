using Autofac;
using Spacearr.Common.Interfaces.Logger;
using Spacearr.Core.Xamarin.Views;
using Spacearr.Pusher.API.Interfaces;
using Spacearr.Pusher.API.Interfaces.Service;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            
            var builder = new ContainerBuilder();
            AutofacConfig.Configure(builder);
            var container = builder.Build();

            #if __ANDROID__
                DependencyService.Get<INotificationManager>().Initialize();
            #endif

            MainPage = new MainPage(container.Resolve<IComputerDriveService>(), container.Resolve<ILogger>(), container.Resolve<IPusherValidation>());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
