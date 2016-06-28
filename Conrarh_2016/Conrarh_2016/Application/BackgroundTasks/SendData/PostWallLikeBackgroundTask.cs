using Conarh_2016.Application.Domain.PostData;
using Conarh_2016.Application.Tools;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.DataAccess;

namespace Conarh_2016.Application.BackgroundTasks
{
	public sealed class PostWallLikeBackgroundTask : PostDataBackgroundTask<AddWallPostLike>
	{
		public readonly User User;
		public readonly WallPost WallPost;

		public PostWallLikeBackgroundTask(User user, WallPost post):
			base(QueryBuilder.Instance.GetPostWallPostLikeQuery (), 
			new AddWallPostLike (user.Id, post.Id),
				true)

		{
			User = user;
			WallPost = post;
		}

		protected override void OnResult (AddWallPostLike result)
		{
			WallPostLike like = new WallPostLike () {
				Id = result.Id,
				CreatedAtTime = result.CreatedAtTime,
				UpdatedAtTime = result.UpdatedAtTime,
				User = User,
				Post = WallPost.Id,
				UserId = User.Id
			};

			AppModel.Instance.WallPostLikes.AddOne (like);
			DbClient.Instance.SaveItemData<WallPostLike> (like).ConfigureAwait(false);
		}
	}
}