using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Tools;
using Conarh_2016.Application.DataAccess;
using System.Collections.Generic;
using System;
using Conarh_2016.Application.BackgroundTasks.GetData.Kinvey;

namespace Conarh_2016.Application.BackgroundTasks
{
    //public sealed class DownloadWallPostLikesBackgroundTask : DownloadListBackgroundTask<WallPostLike, RootListData<WallPostLike>>
    public sealed class DownloadWallPostLikesBackgroundTask : DownloadListKinveyBackgroundTask<WallPostLike, KinveyRootListData<WallPostLike>>
    {
		public readonly WallPost Post;
		public readonly DateTime LastUpdatedTime;

		public DownloadWallPostLikesBackgroundTask(WallPost wallPost, DynamicListData<WallPostLike> data): base(data,
			new KinveyDownloadListParameters( KinveyDownloadCountType.All, 
				QueryBuilder.Instance.GetWallPostsLikesKinveyQuery(wallPost.Id)))
		{
			Post = wallPost;
			LastUpdatedTime = DateTime.Now;
		}

		public DownloadWallPostLikesBackgroundTask(WallPost wallPost, string userId): base(null,
			new KinveyDownloadListParameters(KinveyDownloadCountType.FirstPage, 
				QueryBuilder.Instance.GetWallPostsLikesKinveyQuery(wallPost.Id, userId)))
		{
			Post = wallPost;
		}

		protected override void OnSaveData(List<WallPostLike> data)
		{
			foreach (WallPostLike wallPostLike in data) {
				AppModel.Instance.Users.UpdateData (new List<User>{ wallPostLike.User });
				DbClient.Instance.SaveItemData<User> (wallPostLike.User).ConfigureAwait(false);
			}

			if (TaskParameters.DownloadCountType == KinveyDownloadCountType.All) {
				Post.LastUpdatedLikesTime = LastUpdatedTime;
				DbClient.Instance.SaveItemData<WallPost> (Post).ConfigureAwait (false);
			}
		}
	}
}