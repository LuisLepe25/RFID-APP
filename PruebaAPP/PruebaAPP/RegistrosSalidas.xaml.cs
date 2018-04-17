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
	public partial class RegistrosSalidas : ContentPage
	{
        public List<Log> LstLogsSrc { get; set; }

        public String PathImagenRefresh
        {
            get
            {
                String nombre = "refresh.png";
                String ruta = "";
                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                        ruta = "" + nombre;
                        break;
                    case Device.iOS:
                        ruta = "Icons/" + nombre;
                        break;
                    case Device.UWP:
                        ruta = "Assets/Icons/" + nombre;
                        break;
                }
                return ruta;
            }
        }

        public RegistrosSalidas ()
		{
            try
            {
                BindingContext = this;
                InitializeComponent();
                barraProgreso.ProgressTo(0.5, 500, Easing.Linear);
                ObtenerLogs();
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message != null)
                {
                    DisplayAlert("Alerta", ex.InnerException.Message, "Cerrar");
                }
                else
                {
                    DisplayAlert("Alerta", ex.Message, "Cerrar");
                }
            }
        }

        private async void ObtenerLogs()
        {
            HttpClient client = new HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(5000)
            };
            try
            {
                await barraProgreso.ProgressTo(0.6, 500, Easing.Linear);
                var response = await client.GetStringAsync("http://201.172.20.116/logs/salidas/");
                await barraProgreso.ProgressTo(0.7, 500, Easing.Linear);
                LstLogsSrc = JsonConvert.DeserializeObject<List<Log>>(response);
                LstLogs.ItemsSource = LstLogsSrc;
                await barraProgreso.ProgressTo(1, 500, Easing.Linear);
                if(LstLogsSrc.Count > 0)
                {
                    LstLogs.IsVisible = true;
                    absLayout.IsVisible = false;
                    absVacio.IsVisible = false;
                }
                else
                {
                    absLayout.IsVisible = false;
                    absVacio.IsVisible = true;
                }
            }
            catch (TaskCanceledException ex)
            {
                await DisplayAlert("Alerta", "Tarea cancelada por el sistema. " + ex.InnerException.Message, "Cerrar");
            }
            catch (HttpRequestException ex)
            {
                await DisplayAlert("Alerta", "No se pudo conectar al servidor. " + ex.InnerException.Message, "Cerrar");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alerta", ex.InnerException.Message, "Cerrar");
            }

        }

        private async void LstLogs_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Log l = (Log)e.SelectedItem;
            await DisplayAlert("Elemento", "ID: " + l.ID_Usuario + ", RFID: " + l.RFID + ", Fecha:" + l.Fecha.ToShortDateString(), "Cerrar");
        }

        private async void tItemRefresh_Activated(object sender, EventArgs e)
        {
            await barraProgreso.ProgressTo(0.2, 500, Easing.Linear);
            absVacio.IsVisible = false;
            LstLogs.IsVisible = false;
            absLayout.IsVisible = true;
            this.ObtenerLogs();
        }
    }
}