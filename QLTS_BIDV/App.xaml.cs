using QLTS_BIDV.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QLTS_BIDV
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(StaticConsts.SyncfusionToken);
            //MainPage = new MainPage();
            MainPage = new LoginPage();
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
