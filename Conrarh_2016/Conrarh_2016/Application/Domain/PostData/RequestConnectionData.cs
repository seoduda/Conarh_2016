using Newtonsoft.Json;

namespace Conarh_2016.Application.Domain.PostData
{
	public sealed class RequestConnectionData:UniqueItem
	{
		[JsonProperty(JsonKeys.RequesterId)]
		public string RequesterId;

		[JsonProperty(JsonKeys.ResponderId)]
		public string ResponderId;

        [JsonProperty(JsonKeys.ConnectionRequestId)]
        public string ConnectionRequestId;


        [JsonProperty(JsonKeys.Accepted)]
		public bool Accepted;

		[JsonProperty(JsonKeys.PointsEarned)]
		public int PointsEarned;

		public RequestConnectionData() 
		{
		} 

		public RequestConnectionData(string requesterId, string responderId, int points = 0, bool accepted = false) 
		{
            RequesterId = requesterId;
            ResponderId = responderId;
			PointsEarned = points;
			Accepted = accepted;
		}

        public new static class JsonKeys
        {
            //public const string Responder = "responder";
            //public const string Requester = "requester";
            public const string ResponderId = "responderid";
            public const string RequesterId = "requesterid";
            public const string ConnectionRequestId = "connectionrequestid";
            
            public const string PointsEarned = "points_earned";
            public const string Accepted = "accepted";
        }


    }
}