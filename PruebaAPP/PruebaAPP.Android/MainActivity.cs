using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.LocalNotifications;
using Android.Content;

namespace PruebaAPP.Droid
{
    [Activity(Label = "PruebaAPP", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            //Seleccionamos el icono que tendran las alertas
            LocalNotificationsImplementation.NotificationIconId = Resource.Drawable.alerta;
            Intent notificar = new Intent(this, typeof(NotificarIntentosNoAutorizados));
            StartService(notificar);
            LoadApplication(new App());
        }
    }
}

