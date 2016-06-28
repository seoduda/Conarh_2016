using System;
using PushNotification.Plugin.Abstractions;
using System.Collections.Generic;
using Xamarin.Forms;
using Conarh_2016.Application.Domain;

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
	}
}

