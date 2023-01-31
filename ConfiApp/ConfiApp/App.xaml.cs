using ConfiApp.Modelos;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConfiApp
{
    public partial class App : Application,ILoginManager

    {
        static ILoginManager loginManager;
        public static App Current;
        public static int val;
        public static PerfilUsr PropertiedPopup;
        [Obsolete]
        public static MasterDetailPage MasterDet {get;set; }
        public App()
        {

            InitializeComponent();
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTUyNzYwQDMxMzkyZTM0MmUzMFBqZEN4WHVKYkE0YVFURzZQbnVKRk1kajlnT1R4czl1Y2dCTFNnZlc3NXc9");
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjYwMzk3QDMyMzAyZTMxMmUzMFBYWkF0MG81WlNlUmovRXE4OEV5d2pIMTdNb2VyVzMra1J2YXl4QnRSMGs9");
            Current = this;
            PropertiedPopup = new PerfilUsr();
            PropertiedPopup.CloseWhenBackgroundIsClicked = true;
            PropertiedPopup.IsVisible = false;
            var isLoggedIn = Properties.ContainsKey("IsLoggedIn") ? (bool)Properties["IsLoggedIn"] : false;
            if(isLoggedIn)
            {
                MainPage = new Home();

            }
            else
            {
                MainPage = new LoginModalPage(this);
            }
          //  MainPage = MainPage = new NavigationPage(new MainPage());
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

        [Obsolete]
        public void ShowMainPage()
        {
            MainPage = new Home();

        }

        public void Logout()
        {
            Properties["IsLoggedIn"] = false;
            MainPage = new LoginModalPage(this);
        }
    }

    
}
