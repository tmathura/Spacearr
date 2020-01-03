using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Multilarr.Services;
using Multilarr.Views;

namespace Multilarr
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            DependencyService.Register<MockDriveDataStore>();
            MainPage = new MainPage();
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
