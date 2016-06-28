using Newtonsoft.Json;
using PushNotification.Plugin.Abstractions;
using System.Collections.Generic;

namespace Conarh_2016.Application.Domain
{
	public sealed class PushNotificationData:UpdatedUniqueItem
	{
		[JsonProperty(JsonKeys.Token)]
		public string Token;

		[JsonProperty(JsonKeys.Platform)]
		public string Platform;

		[JsonProperty(JsonKeys.UserId)]
		public string UserId;

		public new static class JsonKeys
		{
			public const string Token = "token";
			public const string Platform = "platform";
			public const string UserId = "user_id";
		}

		public PushNotificationData()
		{
		}

		public PushNotificationData(AppInformation appInformation, string userId)
		{
			UserId = userId;
			Token = appInformation.PushNotificationToken;
			Platform = appInformation.PushNotificationPlatform;
		}
	}
}

//"platform" field is: android or apns
//
//	Request: {
//		"token": "[DEVICE_TOKEN]",
//		"platform": "apns",
//		"user_id": "531cdc460c2eba73e14a28e6"
//	}