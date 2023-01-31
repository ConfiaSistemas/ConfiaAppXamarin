using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Acr.UserDialogs;
using Plugin.LocalNotifications;
//using Android.Support.V4.App;
using Android;


//using Android.Support.V4.Content;
//using Android.Support.V4.Content;

namespace ConfiApp.Droid
{
    [Activity(Label = "ConfiApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private int PERMISSION_REQUEST_CODE = 1;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjYwMzk3QDMyMzAyZTMxMmUzMFBYWkF0MG81WlNlUmovRXE4OEV5d2pIMTdNb2VyVzMra1J2YXl4QnRSMGs9");

            base.OnCreate(savedInstanceState);
            UserDialogs.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Rg.Plugins.Popup.Popup.Init(this);
            LoadApplication(new App());
            LocalNotificationsImplementation.NotificationIconId = Resource.Drawable.LogoConfia;
            PdfSharp.Xamarin.Forms.Droid.Platform.Init();
           


        }

       
       

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}