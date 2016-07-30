using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Conarh_2016.Application.Domain.JsonConverters;
using SQLite.Net.Attributes;

namespace Conarh_2016.Application.Domain
{
	[JsonConverter(typeof(TypedJsonConverter<WallPost>))]
	public sealed class WallPost:UpdatedUniqueItem
	{
		public const string TextPropertyName = "Text";
		public const string LikesPropertyName = "Likes";
		public const string ImagePropertyName = "Image";
		public const string PostDatePropertyName = "PostDate";
		public const string CreatorNamePropertyName = "CreatorName";
        public const string CreatorIdPropertyName = "CreatorId";
        public const string CreatorImagePropertyName = "CreatorImage";
		public const string PostLikesPropertyName = "PostLikes";

		[JsonProperty(JsonKeys.Text)]
		public string Text 
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.Likes)]
		public int Likes
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.Image)]
		public string Image
		{
			set;
			get;
		}

        //[JsonProperty(JsonKeys.CreatedUser)]
        [JsonIgnore]
        [Ignore]
		public User CreatedUser
		{
			set;
			get;
		}
        
        [JsonProperty(JsonKeys.CreatedUserId)]
        public string CreatedUserId
		{
			set;
			get;
		}

		[JsonIgnore]
		[Ignore]
		public List<User> LikeList
		{
			set;
			get;
		}

		[JsonIgnore]
		public DateTime LastUpdatedLikesTime
		{
			set;
			get;
		}

		public new static class JsonKeys
		{
			public const string Text = "text";
			public const string LikeList = "like_list";
			public const string Likes = "likes";
			public const string Image = "image";
			public const string CreatedUserId = "userid";
            public const string CreatedUser = "user";
            public const string CreatedDate = "created_at";
		}

		public override string ToString ()
		{
			return string.Format ("[WallPost] [ Id: {0} Text: {1} UpdatedAt: {2} LikeList: {3} Likes: {4} Image: {5} CreatedUser: {6}]", Id, Text, UpdatedAtTime, "LikeList", Likes, Image, CreatedUser);
		}
			
		public string PostDate 
		{
			get 
			{
				return string.Format("{0}h:{1}m | {2}/{3}/{4}", CreatedAtTime.Hour, CreatedAtTime.Minute, CreatedAtTime.Day, CreatedAtTime.Month, CreatedAtTime.Year);
			}
		}

		public string CreatorName 
		{
			get 
			{
				return CreatedUser.Name;
			}
		}

		public string CreatorImage
        {
            get
            {
                return CreatedUser.ProfileImagePath;
            }
        }
            

            

		public string PostLikes 
		{
			get 
			{
				return string.Format("{0} like{1}", Likes, Likes > 1 ? "s" : string.Empty);
			}
		}

		public WallPost()
		{
			LikeList = new List<User> ();
		}

		public event Action IsChanged;
		public override void UpdateWithItem (UniqueItem item)
		{
			WallPost post = item as WallPost;

			if (post != null) 
			{
				Likes = post.Likes; 
				if (IsChanged != null)
					IsChanged ();
			}
		}

		#region IComparable implementation

		public override int CompareTo (UpdatedUniqueItem other)
		{
			return other.CreatedAtTime.CompareTo (CreatedAtTime);
		}

		#endregion
	}
}