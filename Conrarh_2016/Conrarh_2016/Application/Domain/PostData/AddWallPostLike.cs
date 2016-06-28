using Newtonsoft.Json;

namespace Conarh_2016.Application.Domain.PostData
{
	public sealed class AddWallPostLike:UpdatedUniqueItem
	{
		[JsonProperty(WallPostLike.JsonKeys.Post)]
		public string PostId;

		[JsonProperty(WallPostLike.JsonKeys.User)]
		public string UserId;
	

		public AddWallPostLike()
		{
		}

		public AddWallPostLike( string userId, string postId)
		{
			PostId = postId;
			UserId = userId;
		}
	}
}