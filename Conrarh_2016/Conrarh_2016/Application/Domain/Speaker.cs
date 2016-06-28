using Newtonsoft.Json;
using Conarh_2016.Application.Domain.JsonConverters;

namespace Conarh_2016.Application.Domain
{
	[JsonConverter(typeof(TypedJsonConverter<Speaker>))]
	public sealed class Speaker:UpdatedUniqueItem
	{
		public const string NamePropertyName = "Name";
		public const string ProfileImagePathPropertyName = "ProfileImagePath";
		public const string JobPropertyName = "Job";
		public const string BioPropertyName = "Bio";

		[JsonProperty(JsonKeys.Name)]
		public string Name 
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.Bio, NullValueHandling = NullValueHandling.Ignore)]
		public string Bio
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
			
		[JsonProperty(JsonKeys.Job)]
		public string Job 
		{
			set;
			get;
		}
			
		public new static class JsonKeys
		{
			public const string Bio = "bio";
			public const string Name = "name";
			public const string ProfileImagePath = "profile_image";
			public const string Job = "job_activity";
		}

		public override string ToString ()
		{
			return string.Format ("[Speaker] [ Id: {0} Name: {1}]",  Id, Name);
		}
	}
}