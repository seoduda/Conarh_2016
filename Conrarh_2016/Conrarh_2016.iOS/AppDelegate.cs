using Conarh_2016.Application.DataAccess;
using Conarh_2016.Application.UI;
using Conarh_2016.Core;
using Conarh_2016.iOS.Services;
using Conarh_2016.iOS.UI;
using Foundation;
//using SQLite.Net.Platform.XamarinIOS;
using UIKit;
using Xamarin.Forms;
using XLabs.Platform.Services.Media;
using PushNotification.Plugin;
using System;
using Acr.UserDialogs;
using Conarh_2016.Application;
using Conarh_2016.Core.Services;
using Xamarin.Forms.Platform.iOS;
using SQLite.Net.Platform.XamarinIOS;

namespace Conarh_2016.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : FormsApplicationDelegate
	{
		const string TAG = "PushNotification-APN";
		UIWindow window;
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
            
          //  UserDialogs.Init();


            AppProvider.Log = new Conarh_2016.iOS.Services.Log ();
			AppProvider.IOManager = new IOManager ();
			AppProvider.PopUpFactory = new PopUpFactory ();

			AppProvider.Screen = new ScreenOptions ();
			AppProvider.MediaPicker = new MediaPicker ();
			AppProvider.ImageService = new ImageServiceIOS ();
			AppProvider.ShareService = new ShareServiceIOS ();
			AppProvider.Initialize ();

			Forms.Init ();

			window = new UIWindow (UIScreen.MainScreen.Bounds);
            /*
			CrossPushNotification.Initialize<CrossPushNotificationListener> ();
			CrossPushNotification.Current.Register ();
            */
			window.RootViewController = (new App()).MainPage.CreateViewController ();
			window.MakeKeyAndVisible ();

			DbClient.Instance.Initialize (new SQLitePlatformIOS());
			AppController.Instance.Start();
	
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;

			return true;
		}

		public override void WillEnterForeground (UIApplication uiApplication)
		{
			base.WillEnterForeground (uiApplication);
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
		}

		public override void DidEnterBackground (UIApplication uiApplication)
		{
			base.DidEnterBackground (uiApplication);
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
		}

		public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
		{
			if (CrossPushNotification.Current is IPushNotificationHandler) 
			{
				((IPushNotificationHandler)CrossPushNotification.Current).OnErrorReceived(error);

			}
		}

		public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
			if (CrossPushNotification.Current is IPushNotificationHandler) 
			{
				((IPushNotificationHandler)CrossPushNotification.Current).OnRegisteredSuccess(deviceToken);
			}
		}

		public override void DidRegisterUserNotificationSettings(UIApplication application, UIUserNotificationSettings notificationSettings)
		{
			application.RegisterForRemoteNotifications();
		}

		//* Uncomment if using remote background notifications. To support this background mode,
		//enable the Remote notifications option from the Background modes section of iOS project properties. 
		//(You can also enable this support by including the UIBackgroundModes key with the remote-notification value in your app�s Info.plist file.)
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
			if (CrossPushNotification.Current is IPushNotificationHandler) 
			{
				((IPushNotificationHandler)CrossPushNotification.Current).OnMessageReceived(userInfo);
			}
        }

		public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
		{
			if (CrossPushNotification.Current is IPushNotificationHandler) 
			{
				((IPushNotificationHandler)CrossPushNotification.Current).OnMessageReceived(userInfo);
			}
		}
	}
}

