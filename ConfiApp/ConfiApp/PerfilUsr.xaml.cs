using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConfiApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PerfilUsr : PopupPage

    {
        class Message
        {
            public string Usuario { get; set; }
            public string UsuarioDestino { get; set; }
        }
        public PerfilUsr()
        {

            InitializeComponent();
           
            // Modelos.NotificacionesViewModel vm = new Modelos.NotificacionesViewModel();
            msj();
           // BindingContext = vm;
          //  BindableLayout.SetItemsSource(ContactListView, Modelos.NotificacionesViewModel.NotificacionesList);
          
          
         //   msj();
        }
        public async void msj()
        {
            
        }

        public async void SfButton_ClickedAsync(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync();

        }
    }
}