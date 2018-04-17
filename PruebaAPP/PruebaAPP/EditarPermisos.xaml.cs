using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebServiceRest_RFID.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace PruebaAPP
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditarPermisos : ContentPage
	{
		public Usuario usuInicial;
        public Permiso permInicial;
        public String ID
        {
            get { return permInicial.ID.ToString(); }
            set
            {
                bool ex = int.TryParse(value, out int _id);
                if (ex)
                {
                    permInicial.ID = _id;
                }
                else
                {
                    permInicial.ID = permInicial.ID;
                }
            }
        }
        public String ID_Usuario
        {
            get { return permInicial.ID_Usuario.ToString(); }
            set
            {
                bool ex = int.TryParse(value, out int _idUsuario);
                if (ex)
                {
                    permInicial.ID_Usuario = _idUsuario;
                }
                else
                {
                    permInicial.ID_Usuario = permInicial.ID_Usuario;
                }
            }
        }
        public String ID_Lector
        {
            get { return permInicial.ID_Lector.ToString(); }
            set
            {
                bool ex = int.TryParse(value, out int _idLector);
                if (ex)
                {
                    permInicial.ID_Lector = _idLector;
                }
                else
                {
                    permInicial.ID_Lector = permInicial.ID_Lector;
                }
            }
        }
        public TimeSpan HoraEntrada
        {
            get { return permInicial.Hora_Entrada; }
            set
            {
                permInicial.Hora_Entrada = value;
            }
        }
        public TimeSpan HoraSalida
        {
            get { return permInicial.Hora_Salida; }
            set
            {
                permInicial.Hora_Salida = value;
            }
        }

        public EditarPermisos(Usuario u)
        {
            usuInicial = u;
            InitializeComponent();
            txtID_Usuario.Text = u.ID.ToString();
            txtID_Lector.IsEnabled = true;
            btnGuardar.IsVisible = true;
            btnEditar.IsVisible = false;
            btnEliminar.IsVisible = false;
            lblID.IsVisible = false;
            txtID.IsVisible = false;
        }

        public EditarPermisos(Permiso p)
        {
            permInicial = p;
            BindingContext = this;
            InitializeComponent();
        }

        private async void btnGuardar_Clicked(object sender, EventArgs e)
        {
            bool bIdLector = int.TryParse(txtID_Lector.Text, out int _idLector);
            TimeSpan tEntrada = timeEntrada.Time;
            TimeSpan tSalida = timeSalida.Time;
            //await DisplayAlert("Hora", "Entrada: " + timeEntrada.Time.ToString() + ", Salida: " + timeSalida.Time.ToString(), "Cerrar");
            if (bIdLector)
            {
                HttpClient client = new HttpClient
                {
                    Timeout = TimeSpan.FromMilliseconds(5000)
                };
                try
                {
                    Permiso p = new Permiso
                    {
                        ID_Usuario = usuInicial.ID,
                        ID_Lector = _idLector,
                        Hora_Entrada = tEntrada,
                        Hora_Salida = tSalida
                    };
                    String s = JsonConvert.SerializeObject(p);
                    var contenido = new StringContent(JsonConvert.SerializeObject(p), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("http://201.172.20.116/permiso/crear/", contenido);
                    if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.NoContent)
                    {
                        await DisplayAlert("Creación exitosa", "El permiso se ha creado exitosamente.", "Cerrar");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Algo falló", "El servidor contesto con statusCode " + response.StatusCode + ", probablemente tu solicitud no se realizó correctamente.", "Cerrar");
                    }
                }
                catch (TaskCanceledException ex)
                {
                    if (ex.Message != null)
                    {
                        await DisplayAlert("Alerta", "Tarea cancelada por el sistema. " + ex.Message, "Cerrar");
                    }
                    else
                    {
                        await DisplayAlert("Alerta", "Tarea cancelada por el sistema. " + ex.InnerException.Message, "Cerrar");
                    }
                }
                catch (HttpRequestException ex)
                {
                    if (ex.Message != null)
                    {
                        await DisplayAlert("Alerta", "No se pudo conectar al servidor. " + ex.Message, "Cerrar");
                    }
                    else
                    {
                        await DisplayAlert("Alerta", "No se pudo conectar al servidor. " + ex.InnerException.Message, "Cerrar");
                    }
                }
                catch (Exception ex)
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
        }

        private async void btnEditar_Clicked(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(5000)
            };
            try
            {
                var contenido = new StringContent(JsonConvert.SerializeObject(permInicial), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("http://201.172.20.116/permiso/actualizar/"+permInicial.ID, contenido);
                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.NoContent)
                {
                    await DisplayAlert("Actualización exitosa", "El permiso se ha actualizado exitosamente.", "Cerrar");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Algo falló", "El servidor contesto con statusCode " + response.StatusCode + ", probablemente tu solicitud no se realizó correctamente.", "Cerrar");
                }
            }
            catch (TaskCanceledException ex)
            {
                if (ex.Message != null)
                {
                    await DisplayAlert("Alerta", "Tarea cancelada por el sistema. " + ex.Message, "Cerrar");
                }
                else
                {
                    await DisplayAlert("Alerta", "Tarea cancelada por el sistema. " + ex.InnerException.Message, "Cerrar");
                }
            }
            catch (HttpRequestException ex)
            {
                if (ex.Message != null)
                {
                    await DisplayAlert("Alerta", "No se pudo conectar al servidor. " + ex.Message, "Cerrar");
                }
                else
                {
                    await DisplayAlert("Alerta", "No se pudo conectar al servidor. " + ex.InnerException.Message, "Cerrar");
                }
            }
            catch (Exception ex)
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

        private async void btnEliminar_Clicked(object sender, EventArgs e)
        {
            bool res = await DisplayAlert("Eliminar", "¿Seguro quieres eliminar este permiso?", "Aceptar", "Cancelar");
            if (res)
            {
                HttpClient client = new HttpClient
                {
                    Timeout = TimeSpan.FromMilliseconds(5000)
                };
                try
                {
                    HttpResponseMessage response = await client.DeleteAsync("http://201.172.20.116/permiso/eliminar/" + permInicial.ID);
                    if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.NoContent)
                    {

                        await DisplayAlert("Eliminación exitosa", "El permiso seleccionado se elimino con éxito", "Cerrar");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Algo falló", "El servidor contesto con statusCode " + response.StatusCode + ", probablemente tu solicitud no se realizó correctamente.", "Cerrar");
                    }
                }
                catch (TaskCanceledException ex)
                {
                    if (ex.Message != null)
                    {
                        await DisplayAlert("Alerta", "Tarea cancelada por el sistema. " + ex.Message, "Cerrar");
                    }
                    else
                    {
                        await DisplayAlert("Alerta", "Tarea cancelada por el sistema. " + ex.InnerException.Message, "Cerrar");
                    }
                }
                catch (HttpRequestException ex)
                {
                    if (ex.Message != null)
                    {
                        await DisplayAlert("Alerta", "No se pudo conectar al servidor. " + ex.Message, "Cerrar");
                    }
                    else
                    {
                        await DisplayAlert("Alerta", "No se pudo conectar al servidor. " + ex.InnerException.Message, "Cerrar");
                    }
                }
                catch (Exception ex)
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
        }
    }
}