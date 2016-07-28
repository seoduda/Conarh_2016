using Conarh_2016.Application.Domain.JsonConverters;
using Newtonsoft.Json;
using SQLite.Net.Attributes;

namespace Conarh_2016.Application.Domain
{
        public sealed class FavouriteEventData:UpdatedUniqueItem
	{
		[JsonProperty(JsonKeys.EventId)]
		public string EventId
        {
			set;
			get;
		}

		[JsonProperty(JsonKeys.UserId)]
		public string UserId
        {
			set;
			get;
		}
        /*
        [JsonProperty(JsonKeys.Event)]
        [Ignore]
        public EventData Event
        {
            set;
            get;
        }

    */

        public new static class JsonKeys
		{
			public const string UserId = "userid";
			public const string EventId = "eventid";
            //public const string Event = "event";
        }

		public FavouriteEventData()
		{
		}

		public FavouriteEventData(string userId, string eventId)
		{
            UserId = userId;
            EventId = eventId;
		}
        /*
        public FavouriteEventData(string userId, string eventId, EventData eventdata)
        {
            UserId = userId;
            EventId = eventId;
            Event = eventdata;
        }
        */
    }
}