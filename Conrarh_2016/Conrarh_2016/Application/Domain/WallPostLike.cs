using System;
using Newtonsoft.Json;
using Conarh_2016.Application.Domain.JsonConverters;
using SQLite.Net.Attributes;

namespace Conarh_2016.Application.Domain
{
	[JsonConverter(typeof(TypedJsonConverter<WallPostLike>))]
	public sealed class WallPostLike:UpdatedUniqueItem
	{
		[JsonProperty(JsonKeys.Post)]
		public string Post 
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.User)]
		[Ignore]
		public User User 
		{
			set;
			get;
		}

		[JsonIgnore]
		public string UserId
		{
			set;
			get;
		}

		public new static class JsonKeys
		{
			public const string User = "user";
			public const string Post = "post";
		}
	}
}