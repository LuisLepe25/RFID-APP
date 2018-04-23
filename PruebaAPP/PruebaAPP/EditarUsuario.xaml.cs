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
	public partial class EditarUsuario : ContentPage
	{
        public Usuario usrInicial;
        public String ID
        {
            get { return usrInicial.ID.ToString(); }
            set
            {
                bool ex = int.TryParse(value, out int _id);
                if (ex)
                {
                    usrInicial.ID = _id;
                }
                else
                {
                    usrInicial.ID = usrInicial.ID;
                }
            }
        }
        public String Nombre
        {
            get { return usrInicial.Nombre.ToString(); }
            set { usrInicial.Nombre = value.ToString(); }
        }
        public String RFID
        {
            get { return usrInicial.RFID.ToString(); }
            set
            {
                bool ex = long.TryParse(value, out long _rfid);
                if (ex)
                {
                    usrInicial.RFID = _rfid;
                } else
                {
                    usrInicial.RFID = usrInicial.RFID;
                }
            }
        }
        public EditarUsuario (Usuario usr)
		{
            usrInicial = usr;
            BindingContext = this;
			InitializeComponent ();
		}

        public EditarUsuario()
        {
            InitializeComponent();
            btnGuardar.IsVisible = true;
            btnEditar.IsVisible = false;
            btnEliminar.IsVisible = false;
            txtID.IsVisible = false;
            lblID.IsVisible = false;
        }

        private async void btnGuardar_Clicked(object sender, EventArgs e)
        {
            String Nombre = txtNombre.Text;
            bool bRFID = long.TryParse(txtRFID.Text, out long RFID);
            if (bRFID)
            {
                HttpClient client = new HttpClient
                {
                    Timeout = TimeSpan.FromMilliseconds(5000)
                };
                try
                {

                    Usuario u = new Usuario();
                    u.Nombre = Nombre;
                    u.RFID = RFID;
                    //var response = await client.GetStringAsync("http://localhost:62875/usuario/crear/");
                    var contenido = new StringContent(JsonConvert.SerializeObject(u), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("http://201.172.20.116/usuario/crear/", contenido);
                    if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.NoContent)
                    {
                        await DisplayAlert("Creación exitosa", "El usuario se ha creado exitosamente.", "Cerrar");
                        await Navigation.PopAsync();
                    } else
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
            bool bID = int.TryParse(txtID.Text, out int ID);
            String Nombre = txtNombre.Text;
            bool bRFID = long.TryParse(txtRFID.Text, out long RFID);
            if(bID && bRFID)
            {
                HttpClient client = new HttpClient
                {
                    Timeout = TimeSpan.FromMilliseconds(5000)
                };
                try
                {

                    Usuario u = new Usuario();
                    u.ID = ID;
                    u.Nombre = Nombre;
                    u.RFID = RFID;
                    //var response = await client.GetStringAsync("http://201.172.20.116/usuario/all");
                    var contenido = new StringContent(JsonConvert.SerializeObject(u), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PutAsync("http://201.172.20.116/usuario/actualizar/" + ID, contenido);
                    if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.NoContent)
                    {
                        await DisplayAlert("Actualización exitosa", "La información del usuario se ha actualizado exitosamente.", "Cerrar");
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

        private async void btnEliminar_Clicked(object sender, EventArgs e)
        {
            bool res = await DisplayAlert("Eliminar", "¿Seguro quieres eliminar este usuario?", "Aceptar", "Cancelar");
            if (res)
            {
                HttpClient client = new HttpClient
                {
                    Timeout = TimeSpan.FromMilliseconds(5000)
                };
                try
                {
                    HttpResponseMessage response = await client.DeleteAsync("http://201.172.20.116/usuario/eliminar/" + usrInicial.ID);
                    if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.NoContent)
                    {

                        await DisplayAlert("Eliminación exitosa", "El usuario seleccionado se elimino con éxito", "Cerrar");
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