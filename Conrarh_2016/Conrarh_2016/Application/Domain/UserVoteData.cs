using Newtonsoft.Json;
using SQLite.Net.Attributes;

namespace Conarh_2016.Application.Domain
{
	public enum UserVoteType
	{
		Conteudo = 0,
		Aplicabilidade = 1,
		Speaker = 2
	}

	public sealed class UserVoteData:UpdatedUniqueItem
	{
		[JsonProperty(JsonKeys.Subject, NullValueHandling = NullValueHandling.Ignore)]
		public string Subject
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.Event, NullValueHandling = NullValueHandling.Ignore)]
		public string Event
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.User, NullValueHandling = NullValueHandling.Ignore)]
		public string User
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.Vote)]
		public string Vote
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.UserVoteType, NullValueHandling = NullValueHandling.Ignore)]
		public UserVoteType UserVoteType
		{
			set;
			get;
		}

		[JsonIgnore]
		[Ignore]
		public bool? IsLike
		{
			get 
			{ 
				if(string.IsNullOrEmpty(Vote))
					return null;

				return Vote.Equals ("Like"); 
			}
		}

		public new static class JsonKeys
		{
			public const string User = "user";
			public const string Event = "event";
			public const string Subject = "subject";
			public const string Vote = "vote";
			public const string UserVoteType = "type";
		}	

		public void SetState(bool isLike)
		{
			Vote = isLike ? "Like" : "Dislike";
		}

		public UserVoteData()
		{
		}

		public UserVoteData(UserVoteType voteType, string eventId, string userId, Speaker speaker = null)
		{
			UserVoteType = voteType;
			if (voteType == UserVoteType.Conteudo)
				Subject = AppResources.EventsActionConteudo;
			else if (voteType == UserVoteType.Aplicabilidade)
				Subject = AppResources.EventsActionAplicabilidade;
			else if (voteType == UserVoteType.Speaker)
				Subject = speaker.Id;

			Event = eventId;
			User = userId;
			Vote = null;
		}

		public override void UpdateWithItem (UniqueItem item)
		{
			var userVote = item as UserVoteData;
			if(userVote != null)
			{
				Id = userVote.Id;
				Vote = userVote.Vote;

				if(userVote.UpdatedAtTime > UpdatedAtTime)
					UpdatedAtTime = userVote.UpdatedAtTime;
			}

		}
	}
}
