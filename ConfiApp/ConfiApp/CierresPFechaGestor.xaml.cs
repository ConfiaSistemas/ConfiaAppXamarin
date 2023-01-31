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
    public partial class CierresPFechaGestor : ContentPage
    {
        public List<TicketsPFecha> Resultado;
        private string lista;
        public class TicketsPFecha

        {

            public int id { get; set; }
            public string Caja { get; set; }
            public string Total { get; set; }
            public string Fecha { get; set; }
            public string Hora { get; set; }
            
            public string UsuarioQueRecibe { get; set; }
            
           
        }

        internal class ODataResponse<T>
        {
            public ObservableCollection<T> Value { get; set; }
        }
        private ObservableCollection<TicketsPFecha> searchedCars = new ObservableCollection<TicketsPFecha>();
        private ObservableCollection<TicketsPFecha> searchedTotalPendiente = new ObservableCollection<TicketsPFecha>();
        public ObservableCollection<TicketsPFecha> SearchedCars { get => searchedCars; set { searchedCars = value; OnPropertyChanged("SearchedCars"); } }
        public ObservableCollection<TicketsPFecha> SearchedTotalPendiente { get => searchedTotalPendiente; set { searchedTotalPendiente = value; OnPropertyChanged("SearchedTotalPendiente"); } }
        public ICommand SearchBarTextChangedCommand { get; set; }
        public CierresPFechaGestor()
        {
          
            InitializeComponent();
            MessagingCenter.Subscribe<App>((App)Xamarin.Forms.Application.Current, "RecargarPagosHoy", (sender) => {
                BusyIndicator.IsRunning = !BusyIndicator.IsRunning;
                BindableLayout.SetItemsSource(MyStackList, null);
                GetMoraApis();
            });

            
            UserDialogs.Instance.ShowLoading("Consultando");
           // BusyIndicator.IsRunning = !BusyIndicator.IsRunning;
           // GetMoraApis();

            // MyStackList.BindingContext = MGestor;
           // BusyIndicator.IsRunning = !BusyIndicator.IsRunning;
            this.BindingContext = this;

            UserDialogs.Instance.HideLoading();
            //CV.ItemsSource = MGestor;
        }
     



        public ObservableCollection<TicketsPFecha> MGestor { get; set; }


       


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
            double totalC;
            totalC = 0;
            lista = "MoraApis";
           //UserDialogs.Instance.ShowLoading("Consultando");
            var request = new HttpRequestMessage();
           
            request.RequestUri = new Uri(App.Current.Properties["DDNS"].ToString() + "/ApiCierresGestorPfecha?Usuario=" + App.Current.Properties["name"].ToString() + "&FechaInicio="+ DateInicio.Date.ToString("yyyy-MM-dd") + "&FechaFin=" + DateFin.Date.ToString("yyyy-MM-dd"));
            request.Method = HttpMethod.Get;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
               var MGestors = JsonConvert.DeserializeObject<ObservableCollection<TicketsPFecha>>(content);
                Resultado = JsonConvert.DeserializeObject<List<TicketsPFecha>>(content);
                MGestor = MGestors;
                BindableLayout.SetItemsSource(MyStackList, MGestor);
                // await DisplayAlert("Mensaje", MGestor[0].Multas, "Ok");
               
               foreach(var ass in MGestor)
                {
                   
                        totalC = totalC + double.Parse(ass.Total.Replace("$", string.Empty));
                    
                }
               
            }
            lblCobrado.Text = totalC.ToString("C2", CultureInfo.CurrentCulture);
            //    UserDialogs.Instance.HideLoading();

            BusyIndicator.IsRunning = !BusyIndicator.IsRunning;
           
        }

       



    }


    public class CierresFecha
    {
        public int id { get; set; }
        public string Nombre { get; set; }
       

        public string TotalPendiente { get; set; }

        public ObservableCollection<DetalleCierres> DetallesMora { get; set; }
    }
    public class DetalleCierres
    {
     
        public string Pagaré { get; set; }


        public string PagoSemanal { get; set; }


        public string AbonadoSinMultas { get; set; }
        public string Multas { get; set; }


        public string MultasAbonadas { get; set; }


        public string MultasPendientes { get; set; }


        public string VencidoNormal { get; set; }


       


        public int PagosAtrasados { get; set; }


        public string FechaDelÚltimoAbono { get; set; }
        public int Diasdeatraso { get; set; }


        public string LiquidaCon { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Domicilio { get; set; }
        public string Promotor { get; set; }
        public string Gestor { get; set; }
        public string Estado { get; set; }
    }
}