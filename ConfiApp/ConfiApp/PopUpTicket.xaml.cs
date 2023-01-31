using Acr.UserDialogs;
using ConfiApp.Modelos;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConfiApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpTicket : PopupPage
    {
        string _nmUsuario;
        string _Usuario;
        int _id;
        string _PageOrigen;
        double _TotalApagar;
        public PopUpTicket(int id,double TotalApagar,string Usuario,string nmUsuario,string PageOrigen)
        {
           
            InitializeComponent();
            _id = id;
            _TotalApagar = TotalApagar;
            _Usuario = Usuario;
            _nmUsuario = nmUsuario;
            _PageOrigen = PageOrigen;
            lblID.Text = "ID: " + _id.ToString();
            lblTotalApagar.Text = "Total a pagar: " +  _TotalApagar.ToString("C2", CultureInfo.CurrentCulture);
            txtTotalApagar.Text = _TotalApagar.ToString();
        }

        [Obsolete]
        private async void btn_Clicked(object sender, EventArgs e)
        {
           
            await PopupNavigation.PopAsync();
        }

        [Obsolete]
        private async void btn_Clicked_Pagar(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtTotalApagar.Text))
            {

                await DisplayAlert("Campo vacío", "No puede estar vacío el campo Pago", "Ok");
                txtTotalApagar.Focus();
            }
            else
            {
                if (string.IsNullOrEmpty(txtRecibo.Text))
                {
                    await DisplayAlert("Campo vacío", "No puede estar vacío el campo Recibo", "Ok");
                    txtRecibo.Focus();
                }
                else
                {
                    if(Convert.ToDouble(txtTotalApagar.Text.ToString()) < _TotalApagar  )
                    {
                        string action = await DisplayActionSheet("El valor recibido es menor al total a pagar ¿Desea aplicarlo como abono?", "Cancelar", null, "Sí", "No");
                        if(action=="Sí")
                        {
                            Uri RequestUri = new Uri(App.Current.Properties["DDNS"].ToString() + "/ApiInsertarTicketCobranzaTodos");
                            var client = new HttpClient();
                            //var json = JsonConvert.SerializeObject(mem);

                            var content = new StringContent("idCredito=" + _id + "&TotalApagar=" + _TotalApagar + "&usuario=" + _Usuario + "&nmUsuario=" + _nmUsuario + "&Recibido=" + txtTotalApagar.Text + "&ReciboManual=" + txtRecibo.Text, Encoding.UTF8, "application/x-www-form-urlencoded");
                            // var contentJSON = new StringContent(json, Encoding.UTF8, "application/json");
                            var response = await client.PostAsync(RequestUri, content);
                            response.EnsureSuccessStatusCode();



                            // display the output in TextView
                            //  await DisplayAlert("Mensaje", post.ToString(), "Ok");

                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var resultString = await response.Content.ReadAsStringAsync();
                                var post = JsonConvert.DeserializeObject<ModelTicket>(resultString);
                                UserDialogs.Instance.HideLoading();
                                await DisplayAlert("Datos", "Se insertó el ticket correctamente id " + post.returnValue, "OK");

                                App.MasterDet.IsPresented = false;
                                await App.MasterDet.Detail.Navigation.PushAsync(new Ticket(post.returnValue));
                                if (_PageOrigen == "Mora")
                                {
                                    MessagingCenter.Send((App)Xamarin.Forms.Application.Current, "RecargarMora");
                                }
                                else
                                {
                                    MessagingCenter.Send((App)Xamarin.Forms.Application.Current, "RecargarPagosHoy");
                                }

                                await PopupNavigation.PopAsync();
                                // BusyIndicator.IsRunning = !BusyIndicator.IsRunning;

                            }
                            else
                            {
                                UserDialogs.Instance.HideLoading();
                                await DisplayAlert("Datos", "Ocurrió un error", "OK");
                            }
                        }
                        else
                        {

                        }

                    }
                    else
                    {
                        var request = new HttpRequestMessage();
                        
                            Uri RequestUri = new Uri(App.Current.Properties["DDNS"].ToString() + "/ApiInsertarTicketCobranzaTodos");
                            var client = new HttpClient();
                            //var json = JsonConvert.SerializeObject(mem);

                            var content = new StringContent("idCredito=" + _id + "&TotalApagar=" + _TotalApagar + "&usuario=" + _Usuario + "&nmUsuario=" + _nmUsuario + "&Recibido=" + txtTotalApagar.Text + "&ReciboManual=" + txtRecibo.Text, Encoding.UTF8, "application/x-www-form-urlencoded");
                            // var contentJSON = new StringContent(json, Encoding.UTF8, "application/json");
                            var response = await client.PostAsync(RequestUri, content);
                            response.EnsureSuccessStatusCode();



                            // display the output in TextView
                            //  await DisplayAlert("Mensaje", post.ToString(), "Ok");

                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var resultString = await response.Content.ReadAsStringAsync();
                                var post = JsonConvert.DeserializeObject<ModelTicket>(resultString);
                                UserDialogs.Instance.HideLoading();
                                await DisplayAlert("Datos", "Se insertó el ticket correctamente id " + post.returnValue, "OK");

                                App.MasterDet.IsPresented = false;
                                await App.MasterDet.Detail.Navigation.PushAsync(new Ticket(post.returnValue));
                                if (_PageOrigen == "Mora")
                                {
                                    MessagingCenter.Send((App)Xamarin.Forms.Application.Current, "RecargarMora");
                                }
                                else
                                {
                                    MessagingCenter.Send((App)Xamarin.Forms.Application.Current, "RecargarPagosHoy");
                                }

                                await PopupNavigation.PopAsync();
                                // BusyIndicator.IsRunning = !BusyIndicator.IsRunning;

                            }
                            else
                            {
                                UserDialogs.Instance.HideLoading();
                                await DisplayAlert("Datos", "Ocurrió un error", "OK");
                            }
                        
                       


                    }



                }

            }
           
        }
    }
}