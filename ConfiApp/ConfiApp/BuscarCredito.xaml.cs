using Newtonsoft.Json;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
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

namespace ConfiApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuscarCredito : PopupPage
    {
        public class CreditosEncontrados
        {
            public int id { get; set; }
            public string Nombre { get; set; }
            public string Estado { get; set; }
        }
        public BuscarCredito()
        {
           
            InitializeComponent();
        }
        public ObservableCollection<CreditosEncontrados> mEncontrados;
        private void txtNombre_Completed(object sender, EventArgs e)
        {
            GetEncontrarCredito();

        }

        public async void GetEncontrarCredito()
        {

        //    lista = "MoraApis";
            //UserDialogs.Instance.ShowLoading("Consultando");
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(App.Current.Properties["DDNS"].ToString() + "/ApiBuscarCredito?nombreCredito=" + txtNombre.Text.ToString());
            request.Method = HttpMethod.Get;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                var MGestors = JsonConvert.DeserializeObject<ObservableCollection<CreditosEncontrados>>(content);
               // Resultado = JsonConvert.DeserializeObject<List<MoraApi>>(content);
                mEncontrados = MGestors;
                BindableLayout.SetItemsSource(MyStackList, mEncontrados);
                // await DisplayAlert("Mensaje", MGestor[0].Multas, "Ok");

            }
            //    UserDialogs.Instance.HideLoading();

            //BusyIndicator.IsRunning = !BusyIndicator.IsRunning;

        }

        [Obsolete]
        private async void TapGestureRecognizer_TappedAsync(object sender, EventArgs e)
        {
            StackLayout stck = (StackLayout)sender;
            App.Current.Properties["idCreditoBuscado"] = stck.ClassId.ToString();
            // await DisplayAlert("Mensaje", stck.ClassId.ToString(),"Ok");
            MessagingCenter.Send((App)Xamarin.Forms.Application.Current, "Buscado");
            await PopupNavigation.PopAsync();
        }

        [Obsolete]
        private async void btnCerrar_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync();
        }
    }
}