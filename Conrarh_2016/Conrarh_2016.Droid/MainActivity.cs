using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics.Drawables;
using Android.OS;
using Conarh_2016.Core;
using Conarh_2016.Android.Services;
using Conarh_2016.Android.UI;
using Conarh_2016.Application.UI;
using XLabs.Platform.Services.Media;
using Conarh_2016.Droid.Services;
using TwinTechs.Droid.Controls;
using Xamarin.Forms;
using Conarh_2016.Application.DataAccess;
using Conarh_2016.Application;
using Conarh_2016.Core.Services;
using PushNotification.Plugin;
using SQLite.Net.Platform.XamarinAndroid;

namespace Conarh_2016.Droid
{

    [Activity(Label = "CONARH 2016", Icon = "@drawable/icon", MainLauncher = false, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            AppProvider.Log = new Conarh_2016.Android.Services.Log();
            AppProvider.IOManager = new IOManager();
            AppProvider.PopUpFactory = new PopUpFactory();

            AppProvider.Screen = new ScreenOptions();
            AppProvider.MediaPicker = new MediaPicker();

            AppProvider.ShareService = new ShareServiceAndroid();
            AppProvider.ImageService = new ImageServiceAndroid();
            AppProvider.ImageCache = new ImageLoaderCache();
            AppProvider.LinkedinLogin = new LinkedinLogin();
            AppProvider.FastCellCache = FastCellCache.Instance;
            AppProvider.Initialize();

            Forms.Init(this, bundle);
            UserDialogs.Init(() => (Activity)Forms.Context);


            CrossPushNotification.Initialize<CrossPushNotificationListener>("241198731184");
            CrossPushNotification.Current.Register();

            StartPushService();

            LoadApplication(new App());

            ActionBar.SetIcon(new ColorDrawable(global::Android.Graphics.Color.Transparent));

            DbClient.Instance.Initialize(new SQLitePlatformAndroid());

            AppController.Instance.Start();
        }

        
        public static void StartPushService()
        {
            Forms.Context.StartService(new Intent(Forms.Context, typeof(PushNotificationService)));

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
            {
                PendingIntent pintent = PendingIntent.GetService(Forms.Context, 0, new Intent(Forms.Context, typeof(PushNotificationService)), 0);
                AlarmManager alarm = (AlarmManager)Forms.Context.GetSystemService(Context.AlarmService);
                alarm.Cancel(pintent);
            }

        }

        public static void StopPushService()
        {
            Forms.Context.StopService(new Intent(Forms.Context, typeof(PushNotificationService)));
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
            {
                PendingIntent pintent = PendingIntent.GetService(Forms.Context, 0, new Intent(Forms.Context, typeof(PushNotificationService)), 0);
                AlarmManager alarm = (AlarmManager)Forms.Context.GetSystemService(Context.AlarmService);
                alarm.Cancel(pintent);
            }
          
        }
        

        }

        /*
        [Activity (Label = "CONARH 2016", Icon = "@drawable/icon", MainLauncher = false, ScreenOrientation = ScreenOrientation.Portrait)]
        public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
        {
            int count = 1;

            protected override void OnCreate (Bundle bundle)
            {
                base.OnCreate (bundle);

                // Set our view from the "main" layout resource
                SetContentView (Resource.Layout.Main);

                // Get our button from the layout resource,
                // and attach an event to it
                Button button = FindViewById<Button> (Resource.Id.myButton);

                button.Click += delegate {
                    button.Text = string.Format ("{0} clicks!", count++);
                };
            }
        }
        */
    }


