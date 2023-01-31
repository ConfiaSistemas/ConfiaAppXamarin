using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Telephony;
using ConfiApp.Modelos;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConfiApp
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarioPendiente : ContentPage
    {
        public List<CalPendiente> Resultado;
        private string lista;
        public class CalPendiente
        {

            public int idPago { get; set; }
            public int Npago { get; set; }
            public double Monto { get; set; }
            public double Interes { get; set; }
            public double Abonado { get; set; }
            public double Pendiente { get; set; }
            public DateTime FechaPago { get; set; }
            public DateTime FechaUltimoPago { get; set; }
            public string Estado { get; set; }
            public Boolean Pagar { get; set; }
        }

        public class NombreCredito
        {

          
            public string Nombre { get; set; }
           
        }
        double totalApagar;

        string _idCredito;
        public CalendarioPendiente(string idCredito)
        {

            MessagingCenter.Subscribe<App>((App)Xamarin.Forms.Application.Current, "Buscado", (sender) => {
                txtIdCredito.Text = App.Current.Properties["idCreditoBuscado"].ToString();
                _idCredito = App.Current.Properties["idCreditoBuscado"].ToString();
                //BusyIndicator.IsRunning = !BusyIndicator.IsRunning;
                //  BindableLayout.SetItemsSource(MyStackList, null);
                //GetMoraApis();
            });

            InitializeComponent();
            _idCredito = idCredito;
            txtIdCredito.TextColor = Color.White;
            UserDialogs.Instance.ShowLoading("Consultando");
           
            if (_idCredito == "")
            {

            }
            else
            {
                BusyIndicator.IsRunning = !BusyIndicator.IsRunning;
                txtIdCredito.Text = _idCredito;
                GetMoraApis();
            }
           

            // MyStackList.BindingContext = MGestor;
           // BusyIndicator.IsRunning = !BusyIndicator.IsRunning;
            this.BindingContext = this;

            UserDialogs.Instance.HideLoading();
            //CV.ItemsSource = MGestor;
        }
     



        public ObservableCollection<CalPendiente> MCalendario { get; set; }


       


        private async void btnMora_Clicked(object sender, EventArgs e)
        {
            BusyIndicator.IsRunning = !BusyIndicator.IsRunning;
              BindableLayout.SetItemsSource(MyStackList, null);
              GetMoraApis();


        }
      
       
     
    
        public static bool IsNumeric(object Expression)
        {
            double retNum;

            bool isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }
        public async void GetMoraApis()
        {

            lista = "MoraApis";
            //UserDialogs.Instance.ShowLoading("Consultando");

            var requestNombre = new HttpRequestMessage();
            requestNombre.RequestUri = new Uri(App.Current.Properties["DDNS"].ToString() + "/ApiNombreCredito?idCredito=" + txtIdCredito.Text);
            requestNombre.Method = HttpMethod.Get;
            var clientNombre = new HttpClient();
            HttpResponseMessage responseNombre = await clientNombre.SendAsync(requestNombre);
            if (responseNombre.StatusCode == HttpStatusCode.OK)
            {
                string content = await responseNombre.Content.ReadAsStringAsync();
                var MNombre = JsonConvert.DeserializeObject<ObservableCollection<NombreCredito>>(content);
                //Resultado = JsonConvert.DeserializeObject<List<NombreCredito>>(content);

                // BindableLayout.SetItemsSource(MyStackList, MCalendario);
                lblNombre.Text = MNombre[0].Nombre;
                // await DisplayAlert("Mensaje", MCalendario[0].Pagar.ToString(), "Ok");

            }



            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(App.Current.Properties["DDNS"].ToString() + "/ApiCalendarioPendiente?idCredito=" + txtIdCredito.Text );
            request.Method = HttpMethod.Get;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
               var MGestors = JsonConvert.DeserializeObject<ObservableCollection<CalPendiente>>(content);
                Resultado = JsonConvert.DeserializeObject<List<CalPendiente>>(content);
                MCalendario = MGestors;
                BindableLayout.SetItemsSource(MyStackList, MCalendario);
              // await DisplayAlert("Mensaje", MCalendario[0].Pagar.ToString(), "Ok");
               
            }
        //    UserDialogs.Instance.HideLoading();
            
            BusyIndicator.IsRunning = !BusyIndicator.IsRunning;
           
        }

      
       

        [Obsolete]
  
        private void checkPagar_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            totalApagar = 0;
            var check = (CheckBox)sender;
            foreach (var ass in MCalendario)
            {
                if (ass.idPago.ToString() == check.ClassId.ToString())
                {
                    ass.Pagar = check.IsChecked;
                    
                }
            }
            foreach (var ass in MCalendario)
            {
                if (ass.Pagar)
                {
                    totalApagar += ass.Pendiente;


                }
            }
            lblTotalApagar.Text = "Total a pagar:" + totalApagar.ToString("C2", CultureInfo.CreateSpecificCulture("es-MX"));

        }

        private void Btn_Pagar_Clicked(object sender, EventArgs e)
        {

           string Usuario = App.Current.Properties["name"].ToString();
           string nmUsuario = App.Current.Properties["nm_completo"].ToString();
            PopUpTicket propertiedPopup = new PopUpTicket(Convert.ToInt32(_idCredito),totalApagar,Usuario,nmUsuario,"Mora");
            propertiedPopup.CloseWhenBackgroundIsClicked = true;

            PopupNavigation.PushAsync( propertiedPopup);
        }

        private void btnBuscar_Clicked(object sender, EventArgs e)
        {
            BuscarCredito propertiedPopup = new BuscarCredito();
            propertiedPopup.CloseWhenBackgroundIsClicked = true;

            PopupNavigation.PushAsync(propertiedPopup);
        }
    }


    
}