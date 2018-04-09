using Newtonsoft.Json;
using Plugin.Connectivity;
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
        public List<Usuario> LstUsuariosSrc { get; set; }

        public AdminUsuarios ()
		{
            try
            {
                BindingContext = this;
                InitializeComponent();
                //CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
                barraProgreso.ProgressTo(0.5, 500, Easing.Linear);
                ObtenerUsuarios();
            } catch (Exception ex)
            {
                DisplayAlert("Alerta", ex.InnerException.Message, "Cerrar");
            }
            
        }
        /*
        private async void Current_ConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            try
            {
                if (!e.IsConnected)
                {
                    await DisplayAlert("Alerta", "Revise su conexion a internet", "Cerrar");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alerta", ex.InnerException.Message, "Cerrar");
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await DisplayAlert("Alerta", "Revise su conexion a internet", "Cerrar");
                }
            } catch (Exception ex)
            {
                await DisplayAlert("Alerta", ex.InnerException.Message, "Cerrar");
            }
            
        }
        */
        private async void ObtenerUsuarios()
        {
            HttpClient client = new HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(5000)
            };
            try
            {
                await barraProgreso.ProgressTo(0.6, 500, Easing.Linear);
                var response = await client.GetStringAsync("http://192.168.1.77/usuario/all");
                await barraProgreso.ProgressTo(0.7, 500, Easing.Linear);
                LstUsuariosSrc = JsonConvert.DeserializeObject<List<Usuario>>(response);
                LstUsuarios.ItemsSource = LstUsuariosSrc;
                await barraProgreso.ProgressTo(1, 500, Easing.Linear);
                LstUsuarios.IsVisible = true;
                absLayout.IsVisible = false;
            } catch (TaskCanceledException ex)
            {
                await DisplayAlert("Alerta", "Tarea cancelada por el sistema. " + ex.InnerException.Message, "Cerrar");
            } catch (HttpRequestException ex)
            {
                await DisplayAlert("Alerta", "No se pudo conectar al servidor. " + ex.InnerException.Message, "Cerrar");
            } catch (Exception ex)
            {
                await DisplayAlert("Alerta", ex.InnerException.Message, "Cerrar");
            }
        }

        private async void LstUsuarios_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Usuario u = (Usuario)e.SelectedItem;
            await Navigation.PushAsync(new EditarUsuario(u));
            //await DisplayAlert("Usuario", "Nombre: " + u.Nombre, "Cerrar");
        }
    }
}