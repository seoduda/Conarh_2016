using Newtonsoft.Json;

namespace Conarh_2016.Application.Domain.PostData
{
	public sealed class RequestConnectionData:UniqueItem
	{
		[JsonProperty(ConnectRequest.JsonKeys.Requester)]
		public string Requester;

		[JsonProperty(ConnectRequest.JsonKeys.Responder)]
		public string Responder;

		[JsonProperty(ConnectRequest.JsonKeys.Accepted)]
		public bool Accepted;

		[JsonProperty(ConnectRequest.JsonKeys.PointsEarned)]
		public int PointsEarned;

		public RequestConnectionData() 
		{
		} 

		public RequestConnectionData(string requesterId, string responderId, int points = 0, bool accepted = false) 
		{
			Requester = requesterId;
			Responder = responderId;
			PointsEarned = points;
			Accepted = accepted;
		} 
	}
}