using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PruebaAPP
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuRegistros : ContentPage
	{
		public MenuRegistros ()
		{
			InitializeComponent ();
		}

        private async void btnEntradas_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistrosEntradas());
        }

        private async void btnSalidad_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistrosSalidas());
        }

        private async void btnLogsUsuario_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Intente mas tarde...", " Lo sentimos, esta funcion aun no se implementa, espere futuras versiones", "Cerrar");
        }
    }
}