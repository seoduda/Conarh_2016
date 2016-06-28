using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Tools;

namespace Conarh_2016.Application.BackgroundTasks
{
	public sealed class DownloadFavouriteEventsBackgroundTask : 
	DownloadListBackgroundTask<FavouriteEventData, RootListData<FavouriteEventData>>
	{
		public readonly User User;

		public DownloadFavouriteEventsBackgroundTask(User user, DynamicListData<FavouriteEventData> data): base(data,
			new DownloadListParameters( DownloadCountType.All, 
				QueryBuilder.Instance.GetUserFavouriteEventQuery (user.Id)))
		{
			User = user;
		}

		public DownloadFavouriteEventsBackgroundTask(string eventId, string userId, 
			DynamicListData<FavouriteEventData> data): base(data,
			new DownloadListParameters( DownloadCountType.FirstPage, 
				QueryBuilder.Instance.GetIsFavouriteEventByUserQuery (userId, eventId)))
		{
		}
	}
}