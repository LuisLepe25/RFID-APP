using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebServiceRest_RFID.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PruebaAPP
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AdminUsuarios : ContentPage
	{
        private bool _estaCargando;

        public bool Cargando
        {
            get { return _estaCargando; }
            set
            {
                _estaCargando = value;
                OnPropertyChanged();
            }
        }

        public AdminUsuarios ()
		{
            _estaCargando = true;
            InitializeComponent ();

            ObtenerUsuarios();
        }

        private async void ObtenerUsuarios()
        {
            HttpClient client = new HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(5000)
            };
            try
            {
                var response = await client.GetStringAsync("http://192.168.1.77/usuario/all");
                var usuarios = JsonConvert.DeserializeObject<List<Usuario>>(response);
                LstUsuarios.ItemsSource = usuarios;
                _estaCargando = false;
            } catch (HttpRequestException ex)
            {
                await DisplayAlert("Alerta", "No se pudo conectar al servidor. " + ex.InnerException.Message, "Cerrar");
            }            
        }
    }
}