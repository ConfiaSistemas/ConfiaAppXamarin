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
    public partial class PagosHoy : ContentPage
    {
        public List<PagosPhoy> Resultado;
        private string lista;
        public class PagosPhoy
        {

            public int idCredito { get; set; }
            public string nombre { get; set; }
            public string Monto { get; set; }
            public string Interes { get; set; }
            public string Abonado { get; set; }
            public string Pendiente { get; set; }
            public int NoPago { get; set; }
            public string Estado { get; set; }
            public string Telefono { get; set; }
            public string Celular { get; set; }
            public string EstadoCredito { get; set; }
        }

        internal class ODataResponse<T>
        {
            public ObservableCollection<T> Value { get; set; }
        }
        private ObservableCollection<PagosPhoy> searchedCars = new ObservableCollection<PagosPhoy>();
        private ObservableCollection<PagosPhoy> searchedTotalPendiente = new ObservableCollection<PagosPhoy>();
        public ObservableCollection<PagosPhoy> SearchedCars { get => searchedCars; set { searchedCars = value; OnPropertyChanged("SearchedCars"); } }
        public ObservableCollection<PagosPhoy> SearchedTotalPendiente { get => searchedTotalPendiente; set { searchedTotalPendiente = value; OnPropertyChanged("SearchedTotalPendiente"); } }
        public ICommand SearchBarTextChangedCommand { get; set; }
        public PagosHoy()
        {
            MessagingCenter.Subscribe<App>((App)Xamarin.Forms.Application.Current, "RecargarPagosHoy", (sender) => {
                BusyIndicator.IsRunning = !BusyIndicator.IsRunning;
                BindableLayout.SetItemsSource(MyStackList, null);
                GetMoraApis();
            });

            SearchBarTextChangedCommand = new Command<object>(OnSearchBarTextChanged);
            InitializeComponent();
            UserDialogs.Instance.ShowLoading("Consultando");
            BusyIndicator.IsRunning = !BusyIndicator.IsRunning;
            GetMoraApis();

            // MyStackList.BindingContext = MGestor;
           // BusyIndicator.IsRunning = !BusyIndicator.IsRunning;
            this.BindingContext = this;

            UserDialogs.Instance.HideLoading();
            //CV.ItemsSource = MGestor;
        }
     



        public ObservableCollection<PagosPhoy> MGestor { get; set; }


       


        private async void btnMora_Clicked(object sender, EventArgs e)
        {
            BusyIndicator.IsRunning = !BusyIndicator.IsRunning;
              BindableLayout.SetItemsSource(MyStackList, null);
              GetMoraApis();


        }
        public  Boolean SendSMS(String phoneNumber, String message)
        {
            try
            {
                // Get the SMS manager
                SmsManager smsManager = SmsManager.Default;
                // Split text message content (SMS length limit)
                IList<string> divideContents = smsManager.DivideMessage(message);

                foreach (string text in divideContents)
                {
                    smsManager.SendTextMessage(phoneNumber, null, text, null, null);
                }
                return true;

            }
            catch(Exception ex)
            {
                 DisplayAlert("Error", ex.Message, "Ok");
                return false;
            }
           

        }
        public async void Handle_Tapped(object sender, EventArgs e)
        {
            /* Label frame = (Label)sender;
             TapGestureRecognizer tapGesture = (TapGestureRecognizer)frame.GestureRecognizers[0];*/
            var te = (TappedEventArgs)e;
            string parameter = (string)te.Parameter;
            var locations = await Geocoding.GetLocationsAsync(parameter.ToString());
            var locationDetails = locations?.FirstOrDefault();
            double latitude = locationDetails.Latitude;  // 📍 Latitude: 33.8153959
            double longitude = locationDetails.Longitude;  // 📍Longitude: -117.9263991
            Location location = new Location(latitude, longitude);
            MapLaunchOptions options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };
            await Map.OpenAsync(location, options);
           
        }
        public async void TappedTelefono(object sender, EventArgs e)
        {
            /* Label frame = (Label)sender;
             TapGestureRecognizer tapGesture = (TapGestureRecognizer)frame.GestureRecognizers[0];*/
            var te = (TappedEventArgs)e;
            string parameter = (string)te.Parameter;
            if (IsNumeric(parameter.ToString()))
            {
                PhoneDialer.Open(parameter.ToString());
            }
           

        }
        public async void TappedCelular(object sender, EventArgs e)
        {
            var lbl = (Label)sender;
            var te = (TappedEventArgs)e;
            string parameter = (string)te.Parameter;
            string action = await DisplayActionSheet("¿Qué desea hacer?", "Cancelar", null, "Llamar", "Enviar mensaje");
            if (action == "Llamar")
            {
               
                if (IsNumeric(parameter.ToString()))
                {
                    PhoneDialer.Open(parameter.ToString());
                }
            }
            else if (action == "Enviar mensaje")
            {
                string idC;
                idC = "";
               if(lista == "MoraApis")
                {
                    foreach (var ass in MGestor)
                    {
                        if (ass.idCredito == Convert.ToInt32(lbl.ClassId))
                        {
                            idC = ass.Pendiente.ToString();
                           
                            break;
                        }
                    }
                }
               else
                {
                    foreach (var ass in searchedCars)
                    {
                        if (ass.idCredito == Convert.ToInt32(lbl.ClassId))
                        {
                            idC = ass.Pendiente.ToString();
                           
                            break;
                        }
                    }
                }
                
                string result = await DisplayPromptAsync("Mensaje", "Mensaje a enviar","Ok","Cancel",null,-1,null, "Se le recuerda que hoy es su pago en Préstamos Confía por " + idC +" se le invita a pagar puntualmente para evitar multas");
              
                if(string.IsNullOrEmpty(result))
                {

                }
                else
                {
                    UserDialogs.Instance.ShowLoading("Enviando Mensaje");
                    if (SendSMS(parameter.ToString(), result))
                    {
                        await DisplayAlert("Mensaje", "El mensaje fue enviado", "Ok");
                        UserDialogs.Instance.HideLoading();
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert("Mensaje", "Hubo un error al enviar el mensaje", "Ok");
                    }
                }
               // sendSMS(parameter.ToString(), result);
            }
            /* Label frame = (Label)sender;
             TapGestureRecognizer tapGesture = (TapGestureRecognizer)frame.GestureRecognizers[0];*/
           


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
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(App.Current.Properties["DDNS"].ToString() + "/ApiPagosHoy?Filtro=Gestor&Nusuario=" + App.Current.Properties["name"].ToString());
            request.Method = HttpMethod.Get;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
               var MGestors = JsonConvert.DeserializeObject<ObservableCollection<PagosPhoy>>(content);
                Resultado = JsonConvert.DeserializeObject<List<PagosPhoy>>(content);
                MGestor = MGestors;
                BindableLayout.SetItemsSource(MyStackList, MGestor);
               // await DisplayAlert("Mensaje", MGestor[0].Multas, "Ok");
               
            }
        //    UserDialogs.Instance.HideLoading();
            
            BusyIndicator.IsRunning = !BusyIndicator.IsRunning;
           
        }

        [Obsolete]
        private async void Button_Clicked_Ticket(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            string a;
            int id;
            double totalApagar;
            string Usuario;
            string nmUsuario;
            a = "";
            id = 0;
            Usuario = App.Current.Properties["name"].ToString();
            nmUsuario = App.Current.Properties["nm_completo"].ToString();
            totalApagar = 0;
            foreach (var ass in MGestor)
            {
                if (ass.idCredito.ToString() == btn.ClassId.ToString())
                {
                    id = ass.idCredito;
                    totalApagar = double.Parse(ass.Pendiente.Replace("$", string.Empty));

                    break;
                }
                else
                {
                    a = "";
                }
            }
            string es;

            es = "Hola mi id es " + a;
            //PopUpTicket propertiedPopup = new PopUpTicket(id, totalApagar, Usuario, nmUsuario, "PagosHoy");
            //propertiedPopup.CloseWhenBackgroundIsClicked = true;

            //PopupNavigation.PushAsync(propertiedPopup);
            App.MasterDet.IsPresented = false;
            await App.MasterDet.Detail.Navigation.PushAsync(new CalendarioPendiente(id.ToString()));
        }
        private async void MensajeMasivo(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Enviando Mensaje");

            if (lista == "MoraApis")
            {
            
                foreach (var ass in MGestor)
                {

                  
                    if (SendSMS(ass.Celular, "Se le recuerda que hoy es su pago en Préstamos Confía por " + ass.Pendiente + " se le invita a pagar puntualmente para evitar multas"))
                    {
                       
                      
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert("Mensaje", "Hubo un error al enviar el mensaje", "Ok");
                    }
                   
                }
                
            }
            else
            {
               
                foreach (var ass in searchedCars)
                {

                    if (SendSMS(ass.Celular, "Se le recuerda que hoy es su pago en Préstamos Confía por " + ass.Pendiente + " se le invita a pagar puntualmente para evitar multas"))
                    {


                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert("Mensaje", "Hubo un error al enviar el mensaje", "Ok");
                    }
                }
               
            }
            UserDialogs.Instance.HideLoading();

        }
       


 


public void OnSearchBarTextChanged(object obj)
        {
            UserDialogs.Instance.ShowLoading("Consultando");
            if (obj is TextChangedEventArgs args)
            {
                lista = "Buscando";
                string filter = args.NewTextValue;
                var listed = MGestor.Where(x => x.nombre.ToLower().Contains(filter.Trim().ToLower())).ToList();
                SearchedCars = new ObservableCollection<PagosPhoy>(listed);
                BindableLayout.SetItemsSource(MyStackList, searchedCars);
            }
            UserDialogs.Instance.HideLoading();
        }

    }


    public class PagosParaHoy
    {
        public int id { get; set; }
        public string Nombre { get; set; }
       

        public string TotalPendiente { get; set; }

        public ObservableCollection<DetallePagos> DetallesMora { get; set; }
    }
    public class DetallePagos
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