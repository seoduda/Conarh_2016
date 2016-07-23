using Conarh_2016.Application.Domain.JsonConverters;
using Conarh_2016.Application.Domain.PostData;
using Newtonsoft.Json;
using System;

namespace Conarh_2016.Application.Domain
{
    [JsonConverter(typeof(TypedJsonConverter<ConnectRequest>))]
    public class ConnectRequest : UpdatedUniqueItem
    {
        public event Action<ConnectRequest> IsChanged;

        /*
		[JsonProperty(JsonKeys.Responder, NullValueHandling = NullValueHandling.Ignore)]
		[Ignore]
		public User Responder
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.Requester, NullValueHandling = NullValueHandling.Ignore)]
		[Ignore]
		public User Requester
		{
			set;
			get;
		}
        */

        [JsonProperty(JsonKeys.ResponderId)]
        public string ResponderId
        {
            set;
            get;
        }

        [JsonProperty(JsonKeys.RequesterId)]
        public string RequesterId
        {
            set;
            get;
        }

        [JsonProperty(JsonKeys.PointsEarned)]
        public int PointsEarned
        {
            set;
            get;
        }

        [JsonProperty(JsonKeys.Accepted)]
        public bool Accepted
        {
            set;
            get;
        }

        public ConnectRequest()
        {
        }


        public new static class JsonKeys
        {
            //public const string Responder = "responder";
            //public const string Requester = "requester";
            public const string ResponderId = "responderid";
            public const string RequesterId = "requesterid";
            public const string PointsEarned = "points_earned";
            public const string Accepted = "accepted";
        }

        /*
		public override string ToString ()
		{
			return string.Format ("[ConnectRequest] [ Id: {0} Responder: {1} Requester: {2} PointsEarned: {3} Accepted: {4}]",
				Id, Responder, Requester, PointsEarned, Accepted);
		}
        */


        public override string ToString()
        {
            return string.Format("[ConnectRequest] [ Id: {0} PointsEarned : {1} Accepted: {2} ]",
                Id, PointsEarned, Accepted);
        }

        public override void UpdateWithItem(UniqueItem item)
        {
            ConnectRequest request = item as ConnectRequest;

            if (request != null && request.UpdatedAtTime > UpdatedAtTime)
            {
                Accepted = request.Accepted;
                UpdatedAtTime = request.UpdatedAtTime;
                PointsEarned = request.PointsEarned;
                if (IsChanged != null)
                    IsChanged(this);
            }
        }

    }
}