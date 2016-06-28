using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Tools;
using System.Collections.Generic;
using Conarh_2016.Application.DataAccess;

namespace Conarh_2016.Application.BackgroundTasks
{
	public sealed class DownloadWallPostsBackgroundTask : DownloadListBackgroundTask<WallPost, RootListData<WallPost>>
	{
		public DownloadWallPostsBackgroundTask(DynamicListData<WallPost> data): base(data,
			new DownloadListParameters( DownloadCountType.All, QueryBuilder.Instance.GetWallPostsQuery ()))
		{
		}

		protected override void OnSaveData(List<WallPost> data)
		{
			foreach (WallPost wallPost in data) {
				AppModel.Instance.Users.UpdateData (new List<User>{ wallPost.CreatedUser });
				DbClient.Instance.SaveItemData<User> (wallPost.CreatedUser).ConfigureAwait(false);
			}
		}
	}
}