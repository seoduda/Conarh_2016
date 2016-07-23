using System;
using PushNotification.Plugin.Abstractions;
using System.Collections.Generic;
using Xamarin.Forms;
using Conarh_2016.Application.Domain;
using System.Text;
using Conarh_2016.Application.Domain.PostData;

namespace Conarh_2016.Application
{
	public sealed class AppInformation: UniqueItem
	{
		public const string AppDataId = "AppData"; 

		public static Dictionary<DeviceType, string> PlatformIds = new Dictionary<DeviceType, string> {
			{ DeviceType.iOS, "apns" },
			{ DeviceType.Android, "android" },
		};

		public string CurrentUserId
		{
			set;
			get;
		}

		public string CurrentUserPassword
		{
			set;
			get;
		}

		public string PushNotificationToken
		{
			set;
			get;
		}

        public string KinveyApiToken
        {
            set;
            get;
        }


        public string PushNotificationPlatform
		{
			get {
				return PlatformIds [PushNotificationDeviceType];
			}
		}
			
		private DeviceType PushNotificationDeviceType;

		public AppInformation()
		{
			Id = AppDataId;

			PushNotificationDeviceType = Device.OS == TargetPlatform.iOS ? DeviceType.iOS : DeviceType.Android;
		}

        /*
        public static string SetAPiToken(LoginUserData data)
        {
            StringBuilder sb = new StringBuilder("Basic ");
            sb.Append(getKinveyAuthString(data));
            return sb.ToString();
        }

        private static string getKinveyAuthString(LoginUserData data)
        {
            StringBuilder sb = new StringBuilder(data.Email);
            sb.Append(":");
            sb.Append(data.Password);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            return System.Convert.ToBase64String(plainTextBytes);
        }
        */
    }
}

