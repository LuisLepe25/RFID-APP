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
        public String PathImagenAgregar
        {
            get
            {
                String nombre = "agregar.png";
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
                if(ex.InnerException.Message != null)
                {
                    DisplayAlert("Alerta", ex.InnerException.Message, "Cerrar");
                } else
                {
                    DisplayAlert("Alerta", ex.Message, "Cerrar");
                }
                
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

            //obtener usuarios 
        private async void ObtenerUsuarios()
        {
            HttpClient client = new HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(5000)
            };
            try
            {
                await barraProgreso.ProgressTo(0.6, 500, Easing.Linear);
                //var response = await client.GetStringAsync("http://201.172.20.116/usuario/all");
                var response = await client.GetStringAsync("http://201.172.20.116/usuario/all");
                await barraProgreso.ProgressTo(0.7, 500, Easing.Linear);
                LstUsuariosSrc = JsonConvert.DeserializeObject<List<Usuario>>(response);
                LstUsuarios.ItemsSource = LstUsuariosSrc;
                await barraProgreso.ProgressTo(1, 500, Easing.Linear);
                if (LstUsuariosSrc.Count > 0)
                {
                    LstUsuarios.IsVisible = true;
                    absLayout.IsVisible = false;
                    absVacio.IsVisible = false;
                }
                else
                {
                    absLayout.IsVisible = false;
                    absVacio.IsVisible = true;
                }
            } catch (TaskCanceledException ex)
            {
                if (ex.Message != null)
                {
                    await DisplayAlert("Alerta", "Tarea cancelada por el sistema. " + ex.Message, "Cerrar");
                }
                else
                {
                    await DisplayAlert("Alerta", "Tarea cancelada por el sistema. " + ex.InnerException.Message, "Cerrar");
                }
            } catch (HttpRequestException ex)
            {
                if(ex.Message != null)
                {
                    await DisplayAlert("Alerta", "No se pudo conectar al servidor. " + ex.Message, "Cerrar");
                }
                else
                {
                    await DisplayAlert("Alerta", "No se pudo conectar al servidor. " + ex.InnerException.Message, "Cerrar");
                }
            } catch (Exception ex)
            {
                if (ex.Message != null)
                {
                    await DisplayAlert("Alerta", ex.Message, "Cerrar");
                }
                else
                {
                    await DisplayAlert("Alerta", ex.InnerException.Message, "Cerrar");
                }
            }
        }

        private async void LstUsuarios_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Usuario u = (Usuario)e.SelectedItem;
            await Navigation.PushAsync(new EditarUsuario(u));
        }

        private async void tItemRefresh_Activated(object sender, EventArgs e)
        {
            await barraProgreso.ProgressTo(0.2, 500, Easing.Linear);
            absVacio.IsVisible = false;
            LstUsuarios.IsVisible = false;
            absLayout.IsVisible = true;
            this.ObtenerUsuarios();
        }

        private async void tItemCrear_Activated(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditarUsuario());
        }
    }
}