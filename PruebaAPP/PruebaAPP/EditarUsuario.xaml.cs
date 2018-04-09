using System;
using System.Collections.Generic;
using System.Linq;
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

        private async void btnGuardar_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Objeto mandado", "Nombre: " + usrInicial.Nombre + ", RFID: " + usrInicial.RFID, "Cerrar");
        }
    }
}