using ConfiApp.Modelos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.LocalNotifications;
using Plugin.LocalNotification;
using Rg.Plugins.Popup.Services;

namespace ConfiApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [Obsolete]
    public partial class Home : MasterDetailPage
    {
        private string nombreUsuario;
        public static string NombreUsuario;
        User model2 = new User(NombreUsuario);
        public class Notificaciones
        {
            public int id { get; set; }
            public string Tipo { get; set; }
            public bool Notificacion { get; set; }
            public string Usuario { get; set; }
            public string UsuarioDestino { get; set; }
            public string Valor { get; set; }
            public string Mensaje { get; set; }
            public string Comentario { get; set; }
            public List<Notificaciones> arre { get; set; }
            public bool Aplicado { get; set; }
            public int idSesion { get; set; }
            public string Fecha { get; set; }
            public string Hora { get; set; }
            public bool Visto { get; set; }
            public string FechaAplicacion { get; set; }
            public string HoraAplicacion { get; set; }
            public string Empresa { get; set; }
            public string ComentarioUsuarioDestino { get; set; }
            public string Estado { get; set; }
        }
        public List<Notificaciones> Resultado;
        public List<Notificaciones> ResultadoNuevo;
        public ObservableCollection<Notificaciones> ONotificacion { get; set; }
        public Home()
        {
            App.PropertiedPopup.Opacity = 0;
            App.PropertiedPopup.IsVisible = false;
          //  PopupNavigation.PushAsync(App.PropertiedPopup);

            Resultado = new List<Notificaciones>();

           //Device.StartTimer(new TimeSpan(0, 0, 10), () =>
           // {
           //     // do something every 60 seconds
           //     Device.BeginInvokeOnMainThread(() =>
           //     {
           //         // interact with UI elements
           //         GetNotificacionApi();
           //     });
           //     return true; // runs again, or false to stop
           // });
            nombreUsuario = model2.PruebaNombre();
            InitializeComponent();
            this.Master = new Master();
            this.Detail = new NavigationPage(new Detail());
            App.MasterDet = this;
            NavigationPage.SetHasNavigationBar(this, false);
        
        }
        public async void GetNotificacionApi()
        {

            
            //UserDialogs.Instance.ShowLoading("Consultando");
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(App.Current.Properties["DDNS"].ToString() + "/ApiNotificacionesUsuario?Usuario=" + App.Current.Properties["name"].ToString());
            request.Method = HttpMethod.Get;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                var MGestors = JsonConvert.DeserializeObject<ObservableCollection<Notificaciones>>(content);
                ResultadoNuevo = JsonConvert.DeserializeObject<List<Notificaciones>>(content);
                ONotificacion = MGestors;
                //NotificacionesViewModel.NotificacionesList = JsonConvert.DeserializeObject<ObservableCollection<Modelos.Notificaciones>>(content);
                // BindableLayout.SetItemsSource(MyStackList, MGestor);
                // await DisplayAlert("Mensaje", MGestor[0].Multas, "Ok");
                int i;
                i = 0;
                int n;
                n = 0;

                if (Resultado.Count == 0)
                {
                    

                    if (ResultadoNuevo.Count>0)
                    {
                        Resultado = ResultadoNuevo;
                        foreach (var ass in Resultado)
                        {
                            i += 1;
                        }
                        App.Current.Properties["canNotificaciones"] = i.ToString();
                        MessagingCenter.Send((App)Xamarin.Forms.Application.Current, "RecargarNotificaciones");
                        //  CrossLocalNotifications.Current.Show("ConfiAPP", "Tienes " + i.ToString() + " Notificaciones", 0, DateTime.Now);
                        var notification = new NotificationRequest
                        {
                            NotificationId = 100,
                            Title = "ConfiApp",
                            Description = "Tienes " + i.ToString() + " Notificaciones",
                            ReturningData = "Dummy data", // Returning data when tapped on notification.
                            Android =
                        {

                            IconLargeName =
                            {
                                ResourceName="Confia_Logo_Circulo.png"
                            }
                        }
                        };
                        await NotificationCenter.Current.Show(notification);
                        //  await DisplayAlert("Mensaje", "Tienes " + i.ToString() + " Notificaciones", "Ok");
                    }
                    else
                    {
                        i = 0;
                        App.Current.Properties["canNotificaciones"] = i.ToString();
                        MessagingCenter.Send((App)Xamarin.Forms.Application.Current, "RecargarNotificaciones");
                    }

                   
                  
                }
                else
                {
                    if(ResultadoNuevo.Count>Resultado.Count)
                    {
                        Resultado = ResultadoNuevo;
                        foreach (var ass in Resultado)
                        {
                            i += 1;
                        }
                        var notification = new NotificationRequest
                        {
                            NotificationId = 100,
                            Title = "ConfiApp",
                            Description = "Tienes " + i.ToString() + " Notificaciones",
                            ReturningData = "Dummy data", // Returning data when tapped on notification.
                            Android =
                        {

                            IconLargeName =
                            {
                                ResourceName="Confia_Logo_Circulo.png"
                            }
                        }
                        };
                        await NotificationCenter.Current.Show(notification);
                        App.Current.Properties["canNotificaciones"] = i.ToString();
                        MessagingCenter.Send((App)Xamarin.Forms.Application.Current, "RecargarNotificaciones");
                        //  CrossLocalNotifications.Current.Show("ConfiAPP", "Tienes " + i.ToString() + " Notificaciones", 0);
                        //  await DisplayAlert("Mensaje", "Tienes " + i.ToString() + " Notificaciones", "Ok");
                    }
                    else if(ResultadoNuevo.Count<Resultado.Count)
                        {
                        Resultado = ResultadoNuevo;
                        foreach (var ass in Resultado)
                        {
                            i += 1;
                        }
                        var notification = new NotificationRequest
                        {
                            NotificationId = 100,
                            Title = "ConfiApp",
                            Description = "Tienes " + i.ToString() + " Notificaciones",
                            ReturningData = "Dummy data", // Returning data when tapped on notification.
                            Android =
                        {

                            IconLargeName =
                            {
                                ResourceName="Confia_Logo_Circulo.png"
                            }
                        }
                        };
                        await NotificationCenter.Current.Show(notification);
                        App.Current.Properties["canNotificaciones"] = i.ToString();
                        MessagingCenter.Send((App)Xamarin.Forms.Application.Current, "RecargarNotificaciones");
                        //  CrossLocalNotifications.Current.Show("ConfiAPP", "Tienes " + i.ToString() + " Notificaciones", 0);
                        //  await DisplayAlert("Mensaje", "Tienes " + i.ToString() + " Notificaciones", "Ok");

                    }
                    else
                    {

                    }
                 

                }




            }
            //    UserDialogs.Instance.HideLoading();

          //  BusyIndicator.IsRunning = !BusyIndicator.IsRunning;

        }
    }
}