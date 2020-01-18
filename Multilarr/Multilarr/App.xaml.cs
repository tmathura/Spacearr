using Autofac;
using Multilarr.Common.Interfaces.Logger;
using Multilarr.Services;
using Multilarr.Views;
using Xamarin.Forms;

namespace Multilarr
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

            MainPage = new MainPage(container.Resolve<IComputerDriveService>(), container.Resolve<ILogger>());
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
