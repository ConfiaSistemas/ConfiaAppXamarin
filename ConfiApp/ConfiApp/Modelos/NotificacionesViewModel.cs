using ConfiApp.Utils;
using Newtonsoft.Json;
using Plugin.LocalNotification;
using Syncfusion.XForms.PopupLayout;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ConfiApp.Modelos
{
    class NotificacionesViewModel : INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Notificaciones> notificacionesList;
        public ICommand LongPressCommandSelection { get; set; }
        private bool displayPopup;
        public nt nto { get; set; }
        public ICommand OpenPopupCommand { get; set; }

        public bool DisplayPopup
        {
            get
            {
                return displayPopup;
            }
            set
            {
                displayPopup = value;
                RaisePropertyChanged("DisplayPopup");
            }
        }


        public ObservableCollection<Notificaciones> NotificacionesList
        {
            get { return notificacionesList; }
            set { notificacionesList = value; OnPropertyChanged("NotificacionesList"); }

        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private SfPopupLayout popupLayout;

        public NotificacionesViewModel()


        {
            nto = new nt();
            OpenPopupCommand = new Command(OpenPopup);
            popupLayout = new SfPopupLayout();
            LongPressCommandSelection = new Command(LongPressCommand_SelectionChanged);
            NotificacionesList = new ObservableCollection<Notificaciones>
           {
               new Notificaciones
               {
                   id=1,
                   Tipo="Noti",
                   Notificacion=true,
                   Usuario="Admin",
                   UsuarioDestino="Moises",
                   Valor="",
                   Mensaje="AAAA",
                   Comentario="SSSS",
                   Aplicado=false,
                   idSesion=0,
                   Fecha="2021-12-24",
                   Hora=DateTime.Now,
                   Visto=false,
                   FechaAplicacion="2021-12-24",
                   HoraAplicacion=DateTime.Now,
                   Empresa="Uruapan",
                   ComentarioUsuarioDestino="",
                   Estado=""

               }
           };
            GetNotificacionApi();
            ClockViewModel();
        }
        private void OpenPopup(object obj)
        {
            DisplayPopup = true;
            var i = (int)obj;
            int s =0;
            for(int a =0; a==notificacionesList.Count-1;a++)
            {
                if(notificacionesList[a].id==i)
                {
                    s = a;
                    break;
                }
            }
            nto.Nombre = notificacionesList[s].Tipo.ToString();
            nto.Nombre2 = notificacionesList[s].UsuarioDestino;

            
        }

        public void LongPressCommand_SelectionChanged()
        {
        
        }
        #region INotifyPropertyChanged

      

        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
        public async void GetNotificacionApi()
        {


            //UserDialogs.Instance.ShowLoading("Consultando");
            //var request = new HttpRequestMessage();
            //request.RequestUri = new Uri(App.Current.Properties["DDNS"].ToString() + "/ApiNotificacionesUsuario?Usuario=" + App.Current.Properties["name"].ToString());
            //request.Method = HttpMethod.Get;
            //var client = new HttpClient();
            //HttpResponseMessage response = await client.SendAsync(request);
            //if (response.StatusCode == HttpStatusCode.OK)
            //{

            //    string content = await response.Content.ReadAsStringAsync();
            //    NotificacionesList = JsonConvert.DeserializeObject<ObservableCollection<Notificaciones>>(content);












            //}
            notificacionesList.Add(new Notificaciones
            {
                id = 1,
                Tipo = "Noti",
                Notificacion = true,
                Usuario = "Admin",
                UsuarioDestino = "eee",
                Valor = "",
                Mensaje = "AAAA",
                Comentario = "SSSS",
                Aplicado = false,
                idSesion = 0,
                Fecha = "2021-12-24",
                Hora = DateTime.Now,
                Visto = false,
                FechaAplicacion = "2021-12-24",
                HoraAplicacion = DateTime.Now,
                Empresa = "Uruapan",
                ComentarioUsuarioDestino = "",
                Estado = ""
            });
        }

        public  void ClockViewModel()
        {
           

         Device.StartTimer(TimeSpan.FromSeconds(20),      () =>
            {
                //notificacionesList.Add(new Notificaciones
                //{
                //    id = 1,
                //    Tipo = "Noti",
                //    Notificacion = true,
                //    Usuario = "Admin",
                //    UsuarioDestino = "Moises",
                //    Valor = "",
                //    Mensaje = "AAAA",
                //    Comentario = "SSSS",
                //    Aplicado = false,
                //    idSesion = 0,
                //    Fecha = "2021-12-24",
                //    Hora = DateTime.Now,
                //    Visto = false,
                //    FechaAplicacion = "2021-12-24",
                //    HoraAplicacion = DateTime.Now,
                //    Empresa = "Uruapan",
                //    ComentarioUsuarioDestino = "",
                //    Estado = ""
                //});
                var isLoggedIn = App.Current.Properties.ContainsKey("IsLoggedIn") ? (bool)App.Current.Properties["IsLoggedIn"] : false;
                if (isLoggedIn)
                {
                    GetNotificaciones();
                }
                else
                {
                  
                }
              
                return true; 
            });
        }


        public async void GetNotificaciones()
        {
            List<Notificaciones> eliminar = new List<Notificaciones>();
            List<Notificaciones> agregar = new List<Notificaciones>();
          
          await  HttpNotificaciones.GetAllNewsAsync(async list =>
            {
                int diferencias = 0;
                foreach (Notificaciones i in NotificacionesList)
                {
                    int s = 0;
                    foreach (Notificaciones a in list)
                    {
                        if (i.id == a.id)
                        {
                            s += 1;

                        }
                    }
                    if (s == 0)
                    {
                      //  Console.WriteLine(i.id.ToString());
                       eliminar.Add(i);
                       // NotificacionesList.Remove(i);
                    }
                }
                foreach (Notificaciones r in eliminar)
                {
                    NotificacionesList.Remove(r);
                }
                  foreach (Notificaciones item in list)
                   {
                       int s = 0;
                       foreach (Notificaciones a in NotificacionesList)
                       {
                           if (item.id == a.id)
                           {
                               s += 1;
                           }
                       }
                       if (s == 0)  
                       {
                           agregar.Add(item);
                           diferencias += 1;
                       }


                   }

                foreach(Notificaciones r in agregar)
                  {
                      NotificacionesList.Add(r);
                  }


                if(diferencias>0)
                {
                    var notification = new NotificationRequest
                    {
                        NotificationId = 100,
                        Title = "ConfiApp",
                        Description = "Tienes " + diferencias.ToString() + " Notificaciones",
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
                }
               
            });
        }

    }
}
