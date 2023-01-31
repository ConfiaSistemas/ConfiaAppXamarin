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
    public partial class Mora : ContentPage
    {
        public List<MoraApi> Resultado;
        private string lista;
        public class MoraApi
        {

            public int id { get; set; }
            public string Nombre { get; set; }
            public string Pagaré { get; set; }

            [JsonProperty("Pago Semanal")]
            public string PagoSemanal { get; set; }

            [JsonProperty("Abonado sin multas")]
            public string AbonadoSinMultas { get; set; }
            public string Multas { get; set; }

            [JsonProperty("Multas abonadas")]
            public string MultasAbonadas { get; set; }

            [JsonProperty("Multas pendientes")]
            public string MultasPendientes { get; set; }

            [JsonProperty("Vencido normal")]
            public string VencidoNormal { get; set; }

            [JsonProperty("Total Pendiente")]
            public string TotalPendiente { get; set; }

            [JsonProperty("Pagos atrasados")]
            public int PagosAtrasados { get; set; }

            [JsonProperty("Fecha del último abono")]
            public string FechaDelÚltimoAbono { get; set; }
            public int Diasdeatraso { get; set; }

            [JsonProperty("Liquida con")]
            public string LiquidaCon { get; set; }
            public string Telefono { get; set; }
            public string Celular { get; set; }
            public string Domicilio { get; set; }
            public string Promotor { get; set; }
            public string Gestor { get; set; }
            public string Estado { get; set; }
            public string Color { get; set; }
            public Boolean Existe { get; set; }
        }

        internal class ODataResponse<T>
        {
            public ObservableCollection<T> Value { get; set; }
        }
        private ObservableCollection<MoraApi> searchedCars = new ObservableCollection<MoraApi>();
        private ObservableCollection<MoraApi> searchedTotalPendiente = new ObservableCollection<MoraApi>();
        public ObservableCollection<MoraApi> SearchedCars { get => searchedCars; set { searchedCars = value; OnPropertyChanged("SearchedCars"); } }
        public ObservableCollection<MoraApi> SearchedTotalPendiente { get => searchedTotalPendiente; set { searchedTotalPendiente = value; OnPropertyChanged("SearchedTotalPendiente"); } }
        public ICommand SearchBarTextChangedCommand { get; set; }
        public Mora()
        {
            MessagingCenter.Subscribe<App>((App)Xamarin.Forms.Application.Current, "RecargarMora", (sender) => {
                BusyIndicator.IsRunning = !BusyIndicator.IsRunning;
                BindableLayout.SetItemsSource(MyStackList, null);
                GetMoraApis();
            });

            SearchBarTextChangedCommand = new Command<object>(OnSearchBarTextChanged);
            InitializeComponent();
           // UserDialogs.Instance.ShowLoading("Consultando");
            BusyIndicator.IsRunning = !BusyIndicator.IsRunning;
            GetMoraApis();

            // MyStackList.BindingContext = MGestor;
           // BusyIndicator.IsRunning = !BusyIndicator.IsRunning;
            this.BindingContext = this;

            //UserDialogs.Instance.HideLoading();
            //CV.ItemsSource = MGestor;
        }
     



        public ObservableCollection<MoraApi> MGestor { get; set; }


        private ObservableCollection<MoraGestor> GetMoraGestor()
        {
            if (Resultado.Count > 0)
            {
                foreach (MoraApi n in Resultado)
                {
                    return new ObservableCollection<MoraGestor>
                {
                    new MoraGestor{id=n.id,Nombre=n.Nombre,TotalPendiente=n.TotalPendiente,DetallesMora=new ObservableCollection<DetalleMora>{ new DetalleMora {Pagaré=n.Pagaré,PagoSemanal=n.PagoSemanal,AbonadoSinMultas=n.AbonadoSinMultas,Multas=n.Multas,MultasAbonadas=n.MultasAbonadas,MultasPendientes=n.MultasPendientes,VencidoNormal=n.VencidoNormal,PagosAtrasados=n.PagosAtrasados,FechaDelÚltimoAbono=n.FechaDelÚltimoAbono,Diasdeatraso=n.Diasdeatraso,LiquidaCon=n.LiquidaCon,Telefono=n.Telefono,Celular=n.Celular,Domicilio=n.Domicilio,Promotor=n.Promotor,Gestor=n.Gestor,Estado=n.Estado  } } }

                };
                }
            }
            else
            {
                return new ObservableCollection<MoraGestor> { new MoraGestor { id = 1, Nombre = "", TotalPendiente = "", DetallesMora = new ObservableCollection<DetalleMora> { new DetalleMora { Pagaré = "", PagoSemanal = "", AbonadoSinMultas = "", Multas = "", MultasAbonadas = "", MultasPendientes = "", VencidoNormal = "", PagosAtrasados = 0, FechaDelÚltimoAbono = "", Diasdeatraso = 0, LiquidaCon = "", Telefono = "", Celular = "", Domicilio = "", Promotor = "", Gestor = "", Estado = "" } } } };
            }
            return new ObservableCollection<MoraGestor> {
                new MoraGestor { id = 1, Nombre = "", TotalPendiente = "", DetallesMora = new ObservableCollection<DetalleMora> { new DetalleMora { Pagaré = "", PagoSemanal = "", AbonadoSinMultas = "", Multas = "", MultasAbonadas = "", MultasPendientes = "", VencidoNormal = "", PagosAtrasados = 0, FechaDelÚltimoAbono = "", Diasdeatraso = 0, LiquidaCon = "", Telefono = "", Celular = "", Domicilio = "", Promotor = "", Gestor = "", Estado = "" } } },
            new MoraGestor { id = 1, Nombre = "", TotalPendiente = "", DetallesMora = new ObservableCollection<DetalleMora> { new DetalleMora { Pagaré = "", PagoSemanal = "", AbonadoSinMultas = "", Multas = "", MultasAbonadas = "", MultasPendientes = "", VencidoNormal = "", PagosAtrasados = 0, FechaDelÚltimoAbono = "", Diasdeatraso = 0, LiquidaCon = "", Telefono = "", Celular = "", Domicilio = "", Promotor = "", Gestor = "", Estado = "" } } },
            new MoraGestor { id = 1, Nombre = "", TotalPendiente = "", DetallesMora = new ObservableCollection<DetalleMora> { new DetalleMora { Pagaré = "", PagoSemanal = "", AbonadoSinMultas = "", Multas = "", MultasAbonadas = "", MultasPendientes = "", VencidoNormal = "", PagosAtrasados = 0, FechaDelÚltimoAbono = "", Diasdeatraso = 0, LiquidaCon = "", Telefono = "", Celular = "", Domicilio = "", Promotor = "", Gestor = "", Estado = "" } } }
            };
        }


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
                UserDialogs.Instance.HideLoading();
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
                        if (ass.id == Convert.ToInt32(lbl.ClassId))
                        {
                            idC = ass.TotalPendiente.ToString();
                           
                            break;
                        }
                    }
                }
               else
                {
                    foreach (var ass in searchedCars)
                    {
                        if (ass.id == Convert.ToInt32(lbl.ClassId))
                        {
                            idC = ass.TotalPendiente.ToString();
                           
                            break;
                        }
                    }
                }
                
                string result = await DisplayPromptAsync("Mensaje", "Mensaje a enviar","Ok","Cancel",null,-1,null,"Se le invita a ponerse al corriente con " + idC +" en Préstamos Confía para evitar multas");
              
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
            request.RequestUri = new Uri(App.Current.Properties["DDNS"].ToString() + "/ApiProfileGet?nombre=" + App.Current.Properties["name"].ToString() );
            request.Method = HttpMethod.Get;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
               var MGestors = JsonConvert.DeserializeObject<ObservableCollection<MoraApi>>(content);
                Resultado = JsonConvert.DeserializeObject<List<MoraApi>>(content);
                MGestor = MGestors;
                BindableLayout.SetItemsSource(MyStackList, MGestor);
               // await DisplayAlert("Mensaje", MGestor[0].Multas, "Ok");
               
            }
        //    UserDialogs.Instance.HideLoading();
            
            BusyIndicator.IsRunning = !BusyIndicator.IsRunning;
           
        }

        private async void MensajeMasivo(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Enviando Mensaje");

            if (lista == "MoraApis")
            {

                foreach (var ass in MGestor)
                {


                    if (SendSMS(ass.Celular, "Se le invita a ponerse al corriente con " + ass.TotalPendiente + " en Préstamos Confía para evitar multas"))
                    {


                    }
                    else
                    {
                       // UserDialogs.Instance.HideLoading();
                        await DisplayAlert("Mensaje", "Hubo un error al enviar el mensaje", "Ok");
                    }

                }

            }
            else
            {

                foreach (var ass in searchedCars)
                {

                    if (SendSMS(ass.Celular, "Se le invita a ponerse al corriente con " + ass.TotalPendiente + " en Préstamos Confía para evitar multas"))
                    {


                    }
                    else
                    {
                      //  UserDialogs.Instance.HideLoading();
                        await DisplayAlert("Mensaje", "Hubo un error al enviar el mensaje", "Ok");
                    }
                }

            }
              UserDialogs.Instance.HideLoading();

        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Trabajando");
            var Btn = sender as Button;
            string result = await DisplayPromptAsync("Gestionar", "Resultado de la gestión");
            if  (string.IsNullOrEmpty(result))
            {
                UserDialogs.Instance.HideLoading();
            }
            else
            {
                GestionCredito mem = new GestionCredito
                {
                    IdCredito = Convert.ToInt32(Btn.ClassId),
                    Gestion = result,
                    IdGestor = 16

                };
                var request = new HttpRequestMessage();
                Uri RequestUri = new Uri(App.Current.Properties["DDNS"].ToString() + "/ApiProfilePost");


                var client = new HttpClient();
                var json = JsonConvert.SerializeObject(mem);

                var content = new StringContent("idCredito=" + mem.IdCredito + "&Gestion=" + mem.Gestion + "&idGestor=" + mem.IdGestor + "", Encoding.UTF8, "application/x-www-form-urlencoded");
                // var contentJSON = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(RequestUri, content);
                response.EnsureSuccessStatusCode();

                // var resultString = await response.Content.ReadAsStringAsync();
                //var post = JsonConvert.DeserializeObject(resultString);

                // display the output in TextView
                //  await DisplayAlert("Mensaje", post.ToString(), "Ok");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Datos", "Se insertó la gestión correctamente", "OK");
                    BusyIndicator.IsRunning = !BusyIndicator.IsRunning;
                    BindableLayout.SetItemsSource(MyStackList,null );
                    GetMoraApis();

                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Datos", "Ocurrió un error", "OK");
                }
            }
            // await DisplayAlert("Mensaje", Btn.ClassId, "Ok");
           
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
                if (ass.id.ToString() == btn.ClassId.ToString())
                {
                    id = ass.id;
                    totalApagar = double.Parse(ass.TotalPendiente.Replace("$",string.Empty));
                    
                        break;
                }
                else
                {
                    a = "";
                }
            }
            string es;
           
            es = "Hola mi id es " + a;
            PopUpTicket propertiedPopup = new PopUpTicket(id,totalApagar,Usuario,nmUsuario,"Mora");
            //propertiedPopup.CloseWhenBackgroundIsClicked = true;

            //PopupNavigation.PushAsync( propertiedPopup);
            App.MasterDet.IsPresented = false;
            await App.MasterDet.Detail.Navigation.PushAsync(new CalendarioPendiente(id.ToString()));
        }



public void OnSearchBarTextChanged(object obj)
        {
            UserDialogs.Instance.ShowLoading("Consultando");
            if (obj is TextChangedEventArgs args)
            {
                lista = "Buscando";
                string filter = args.NewTextValue;
                var listed = MGestor.Where(x => x.Nombre.ToLower().Contains(filter.Trim().ToLower())).ToList();
                SearchedCars = new ObservableCollection<MoraApi>(listed);
                BindableLayout.SetItemsSource(MyStackList, searchedCars);
            }
            UserDialogs.Instance.HideLoading();
        }

    }


    public class MoraGestor
    {
        public int id { get; set; }
        public string Nombre { get; set; }
       

        public string TotalPendiente { get; set; }

        public ObservableCollection<DetalleMora> DetallesMora { get; set; }
    }
    public class DetalleMora
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