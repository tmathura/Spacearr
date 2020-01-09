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

            MainPage = new MainPage(container.Resolve<IComputerDriveDataStore>(), container.Resolve<ILogger>());
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
