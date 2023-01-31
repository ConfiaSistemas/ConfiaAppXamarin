using Android;
using Android.Graphics;
using Android.Graphics.Drawables;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConfiApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Master : ContentPage
    {
        public Master()

        {
            //MessagingCenter.Subscribe<App>((App)Xamarin.Forms.Application.Current, "RecargarNotificaciones", (sender) => {
            //    badgeNotificaciones.BadgeText= App.Current.Properties["canNotificaciones"].ToString();
                
            //  //  lblNotificaciones.Text = App.Current.Properties["canNotificaciones"].ToString();
            //});
            InitializeComponent();
            // var bArray = (byte[])App.Current.Properties["imgI"];
            //var byteArray = DecodeUrlBase64(App.Current.Properties["imgUsr"].ToString());
            if (string.IsNullOrEmpty(App.Current.Properties["imgI"].ToString()))
            {
            
            }
            else
            {
                var by = Convert.FromBase64String(App.Current.Properties["imgI"].ToString());
                Stream stream = new MemoryStream(by);
                var imageSource = ImageSource.FromStream(() => stream);
                imgUser.Source = imageSource;
            }
            
           
            //  Stream stream = new MemoryStream(bArray);
            //   var imageSource = ImageSource.FromStream(() => stream);
            // imgUser.Source = imageSource;
            // txtuser.Text = App.Current.Properties["imgI"].ToString();
            //txtusr.Text = App.Current.Properties["imgUsr"].ToString();
            //    var byteArray = Convert.FromBase64String(App.Current.Properties["imgUsr"].ToString());
            //Stream stream = new MemoryStream(byteArray);
            //var imageSource = ImageSource.FromStream(() => stream);
            //imgUser.Source = imageSource;
          //  imgUser.Source = DisplayImage(App.Current.Properties["imgUsr"].ToString());
            lblUsuario.Text = App.Current.Properties["name"].ToString();
            lblnmUsuario.Text = App.Current.Properties["nm_completo"].ToString();
        }
        public static byte[] StringToByteArray(string hex) { return Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray(); }

        public static ImageSource DisplayImage(string hex) { byte[] imgBytes = StringToByteArray(hex); return ImageSource.FromStream(() => new MemoryStream(imgBytes)); }

        [Obsolete]
        private async void btnMora_Clicked(object sender, EventArgs e)
        {
            

           
            App.MasterDet.IsPresented = false;
            await App.MasterDet.Detail.Navigation.PushAsync(new Mora());

        }

        [Obsolete]
        private async void btnPagosHoy_Clicked(object sender, EventArgs e)
        {

            App.MasterDet.IsPresented = false;
            await App.MasterDet.Detail.Navigation.PushAsync(new PagosHoy());

        }

        [Obsolete]
        private async void btnTickets_Clicked(object sender, EventArgs e)
        {

            App.MasterDet.IsPresented = false;
            await App.MasterDet.Detail.Navigation.PushAsync(new TicketsPFechaGestor());

        }

        [Obsolete]
        private async void btnCierre_Clicked(object sender, EventArgs e)
        {

            App.MasterDet.IsPresented = false;
            await App.MasterDet.Detail.Navigation.PushAsync(new TicketsSinCierre());

        }

        [Obsolete]
        private async void btnCierreFecha_Clicked(object sender, EventArgs e)
        {

            App.MasterDet.IsPresented = false;
            await App.MasterDet.Detail.Navigation.PushAsync(new CierresPFechaGestor());

        }

        private async void btnLogout_Clicked(object sender, EventArgs e)
        {
            App.Current.Logout();

        }

        [Obsolete]
        void OnImageNameTapped(object sender, EventArgs args)
        {
            try
            {
                //PerfilUsr propertiedPopup = new PerfilUsr();

                //  PopupNavigation.PushAsync(propertiedPopup);

                PopupNavigation.PushAsync(App.PropertiedPopup);
                //Code to execute on tapped event
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static byte[] DecodeUrlBase64(string s)
        {
            s = s.Replace('-', '+').Replace('_', '/').PadRight(4 * ((s.Length + 3) / 4), '=');
            return Convert.FromBase64String(s);
        }

        [Obsolete]
        private async void btnAplicarPago_Clicked(object sender, EventArgs e)
        {
            App.MasterDet.IsPresented = false;
            await App.MasterDet.Detail.Navigation.PushAsync(new CalendarioPendiente(""));
        }
    }
}