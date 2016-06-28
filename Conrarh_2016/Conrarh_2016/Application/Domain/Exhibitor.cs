using Conarh_2016.Application.Domain.JsonConverters;
using Newtonsoft.Json;
using SQLite.Net.Attributes;

namespace Conarh_2016.Application.Domain
{
	[JsonConverter(typeof(TypedJsonConverter<Exhibitor>))]
	public sealed class Exhibitor:UpdatedUniqueItem
	{
		[JsonProperty(JsonKeys.Title)]
		public string Title 
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.Venue)]
		public string Venue
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.SponsorType)]
		[Ignore]
		public SponsorType SponsorType
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.Stand)]
		public string Stand
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.Icon)]
		public string Icon
		{
			set;
			get;
		}

		public string SponsorTypeId
		{
			set;
			get;
		}

		public string Description
		{
			get 
			{
				return string.Format ("{0} / Stand {1}", Venue, Stand);
			}
		}

		public new static class JsonKeys
		{
			public const string Title = "title";
			public const string Venue = "venue";
			public const string SponsorType = "sponsor_type";
			public const string Stand = "stand";
			public const string Icon = "icon";
		}

		public override string ToString ()
		{
			return string.Format ("[Exhibitor] [ Id: {0} Title: {1} Venue: {2} SponsorType: {3} Stand: {4} Icon: {5}]", Id, Title, Venue, SponsorType, Stand, Icon);
		}
	}
}