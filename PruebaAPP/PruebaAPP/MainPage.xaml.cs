using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Xamarin.Forms;
using Newtonsoft.Json;
using WebServiceRest_RFID.Models;

namespace PruebaAPP
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

        public async void btnUsuario_Clicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new AdminUsuarios());
        }

        private async void btnLogs_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MenuRegistros());
        }
        private async void btnPermisos_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PermisoListaUsuario());
        }
    }
}
