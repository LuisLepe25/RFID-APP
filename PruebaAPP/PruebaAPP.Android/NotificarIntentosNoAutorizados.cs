using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Plugin.LocalNotifications;

namespace PruebaAPP.Droid
{
    [Service]
    public class NotificarIntentosNoAutorizados : Service
    {
        [get: Register("getMacAddress", "()Ljava/lang/String;", "GetGetMacAddressHandler")]
        public virtual String MacAddress { get; }
        private Timer _Timer;
        public List<WebServiceRest_RFID.Models.Log> LstLogsSrc { get; set; }

        public void DebugApp()
        {
            _Timer = new Timer((o) => { Log.Debug("SS", "Inicie el servicio"); }, null, 0, 2000);
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            Log.Debug("SS", "Inicie el servicio exitosamente...");
            Log.Debug("MAC ADDRESS", MacAddress);
            DebugApp();
            TraerIntentosNoAutorizados();
            return StartCommandResult.Sticky;
        }

        public async void TraerIntentosNoAutorizados()
        {
            HttpClient client = new HttpClient();
            try
            {
                //var response = await client.GetStringAsync("http://201.172.20.116/usuario/all");
                var response = await client.GetStringAsync("http://201.172.20.116/intentos");

                LstLogsSrc = JsonConvert.DeserializeObject<List<WebServiceRest_RFID.Models.Log>>(response);
                if (LstLogsSrc.Count > 0)
                {
                    CrossLocalNotifications.Current.Show("Hubo " + LstLogsSrc.Count + " intentos no autorizados.", "Haga clic para ver.");
                }
            }
            catch (TaskCanceledException ex)
            {
                if (ex.Message != null)
                {
                    Log.Debug("Error", "Tarea cancelada por el sistema." + ex.Message);
                }
                else
                {
                    Log.Debug("Error", "Tarea cancelada por el sistema." + ex.InnerException.Message);
                }
            }
            catch (HttpRequestException ex)
            {
                if (ex.Message != null)
                {
                    Log.Debug("Error", "No se pudo conectar al servidor. " + ex.Message);
                }
                else
                {
                    Log.Debug("Error", "No se pudo conectar al servidor. " + ex.InnerException.Message);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message != null)
                {
                    Log.Debug("Error", ex.Message);
                }
                else
                {
                    Log.Debug("Error", ex.InnerException.Message);
                }
            }
        }
        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }

        public override bool StopService(Intent name)
        {
            CrossLocalNotifications.Current.Show("Pare el servicio", "El servicio aun no funciona si cierras la aplicacion");
            return base.StopService(name);
        }
    }
}