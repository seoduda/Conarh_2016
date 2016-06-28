using Newtonsoft.Json;


namespace Conarh_2016.Application.Domain
{
	public sealed class FavouriteEventData:UpdatedUniqueItem
	{
		[JsonProperty(JsonKeys.Event)]
		public string Event 
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.User)]
		public string User 
		{
			set;
			get;
		}

		public new static class JsonKeys
		{
			public const string User = "user";
			public const string Event = "event";
		}

		public FavouriteEventData()
		{
		}

		public FavouriteEventData(string userId, string eventId)
		{
			User = userId;
			Event = eventId;
		}
	}
}