using Conarh_2016.Application.Domain.JsonConverters;
using Newtonsoft.Json;

namespace Conarh_2016.Application.Domain
{
	[JsonConverter(typeof(TypedJsonConverter<BadgeType>))]
	public sealed class BadgeType:UpdatedUniqueItem
	{
		public const string ImageDisablePropertyName = "BadgeDisableImage";
	
		[JsonProperty(JsonKeys.Title)]
		public string Title
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.BadgeDisableImage)]
		public string BadgeDisableImage
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.BadgeEnableImage)]
		public string BadgeEnableImage
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.Description)]
		public string Description
		{
			set;
			get;
		}

		public new static class JsonKeys
		{
			public const string Title = "title";
			public const string BadgeDisableImage = "badge_disabled";
			public const string BadgeEnableImage = "badge_enabled";
			public const string Description = "description";
		}

		public override string ToString ()
		{
			return string.Format ("[BadgType] [ Id: {0} Title: {1}]", Id, Title);
		}
	}
}
