using Newtonsoft.Json;

namespace Conarh_2016.Application.Domain.PostData
{
	public sealed class CreateWallPostData:UniqueItem
	{
		[JsonProperty(WallPost.JsonKeys.CreatedUserId)]
		public string UserId;

		[JsonProperty(WallPost.JsonKeys.Text)]
		public string Text;

		[JsonProperty(WallPost.JsonKeys.Likes)]
		public int Likes = 0;
	}
}