﻿using Conarh_2016.Application.Domain.JsonConverters;
using Newtonsoft.Json;

namespace Conarh_2016.Application.Domain
{
	[JsonConverter(typeof(TypedJsonConverter<User>))]
	public class User:UpdatedUniqueItem
	{
		public const string NamePropertyName = "Name";
		public const string EmailPropertyName = "Email";
		public const string ProfileImagePathPropertyName = "ProfileImagePath";
		public const string JobPropertyName = "Job";
		public const string ScorePointsPropertyName = "ScorePointsString";
		public const string PhonePropertyName = "Phone";
		public const string LevelImagePathPropertyName = "LevelImagePath";

		[JsonProperty(JsonKeys.UserName)]
		public string Email 
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.Name)]
		public string Name 
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.ProfileImagePath, NullValueHandling = NullValueHandling.Ignore)]
		public string ProfileImagePath 
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.UserType, NullValueHandling = NullValueHandling.Ignore)]
		public string UserType
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.Phone, NullValueHandling = NullValueHandling.Ignore)]
		public string Phone
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.Passphrase)]
		public string Passphrase
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.Job)]
		public string Job 
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.Points)]
		public int ScorePoints 
		{
			set;
			get;
		}

		public string ScorePointsString 
		{
			get {
				return string.Format ("{0} pts", ScorePoints);
			}
		}

		public string LevelImagePath
		{
			get 
			{
				return AppResources.GetLevelImageByPoints (ScorePoints);
			}
		}

		public new static class JsonKeys
		{
			public const string Name = "name";
			public const string UserName = "username";

			public const string ProfileImagePath = "profile_image";

			public const string UserType = "user_type";
			public const string Phone = "phone";
			public const string Points = "points";

			public const string Passphrase = "passphrase";
			public const string Job = "job_activity";

			public const string Password = "password";
		}

		public override string ToString ()
		{
			return string.Format ("[User] [ Id: {0} Name: {1} UserName {2} ]",  Id, Name, Email);
		}

		public override void UpdateWithItem (UniqueItem item)
		{
			User newUserData = item as User ;

			if (newUserData != null) {
				UpdatedAtTime = newUserData.UpdatedAtTime > UpdatedAtTime ? newUserData.UpdatedAtTime : UpdatedAtTime;
				ScorePoints = System.Math.Max(ScorePoints, newUserData.ScorePoints);
			}
		}
	}
}