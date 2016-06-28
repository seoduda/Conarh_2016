using System;
using Conarh_2016.Application;
using Newtonsoft.Json.Linq;
using PushNotification.Plugin;
using PushNotification.Plugin.Abstractions;

namespace Conarh_2016.Core.Services
{
	public class CrossPushNotificationListener:IPushNotificationListener
	{
		#region IPushNotificationListener implementation

		public bool ShouldShowNotification ()
		{
			return true;
		}

		public void OnMessage (System.Collections.Generic.IDictionary<string, object> Parameters, PushNotification.Plugin.Abstractions.DeviceType deviceType)
		{
			AppProvider.Log.WriteLine (LogChannel.Notifications, "OnMessage: {0} / {1}", Parameters, deviceType);
		}

		public void OnRegistered (string Token, DeviceType deviceType)
		{
			AppProvider.Log.WriteLine (LogChannel.Notifications, "Register: {0} / {1}", Token, deviceType);

			AppController.Instance.UpdatePushNotifications (Token, deviceType);
		}

		public void OnUnregistered (PushNotification.Plugin.Abstractions.DeviceType deviceType)
		{
			AppProvider.Log.WriteLine (LogChannel.Notifications, "UnRegister: {0}", deviceType);
		}

		public void OnError (string message, PushNotification.Plugin.Abstractions.DeviceType deviceType)
		{
			AppProvider.Log.WriteLine (LogChannel.Notifications, "OnError: {0} / {1}", message, deviceType);
		}

        public void OnMessage(JObject values, DeviceType deviceType)
        {
            AppProvider.Log.WriteLine(LogChannel.Notifications, "OnMessage: {0} / {1}", values, deviceType);
           
        }

        #endregion
    }
}

