using Newtonsoft.Json;
using Conarh_2016.Application.Domain.JsonConverters;

namespace Conarh_2016.Application.Domain
{
	[JsonConverter(typeof(TypedJsonConverter<SponsorType>))]
	public sealed class SponsorType:UpdatedUniqueItem
	{
		[JsonProperty(JsonKeys.Title)]
		public string Title
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.Color)]
		public string Color
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.Type)]
		public int Type
		{
			set;
			get;
		}

		public new static class JsonKeys
		{
			public const string Title = "title";
			public const string Color = "color";
			public const string Type = "type";
		}

		public override string ToString ()
		{
			return string.Format ("[SponsorType] [ Id: {0} Title: {1}]", Id, Title);
		}
	}
}