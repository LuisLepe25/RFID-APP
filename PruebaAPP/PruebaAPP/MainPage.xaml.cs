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
            //TODO: adminUsuarios no aparece para ponerlo como pagina
            await Navigation.PushAsync(new NavigationPage(new AdminUsuarios()));
        }
    }
}
