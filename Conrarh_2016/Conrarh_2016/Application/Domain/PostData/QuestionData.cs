using Newtonsoft.Json;

namespace Conarh_2016.Application.Domain.PostData
{
	public sealed class QuestionData:UniqueItem
	{
		[JsonProperty(JsonKeys.Event)]
		public string EventId;

		[JsonProperty(JsonKeys.User)]
		public string UserId;

		[JsonProperty(JsonKeys.Question)]
		public string Question;

		public new static class JsonKeys
		{
			public const string Event = "event";
			public const string User = "user";
			public const string Question = "question";
		}
	}
}