using Conarh_2016.Application.BackgroundTasks.GetData.Kinvey;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Tools;

namespace Conarh_2016.Application.BackgroundTasks
{
	public sealed class DownloadFavouriteEventsBackgroundTask :
    GetData.Kinvey.DownloadListKinveyBackgroundTask<FavouriteEventData, KinveyRootListData<FavouriteEventData>>

    //	DownloadListBackgroundTask<FavouriteEventData, RootListData<FavouriteEventData>>
    {
		public readonly User User;

		public DownloadFavouriteEventsBackgroundTask(User user, DynamicListData<FavouriteEventData> data): base(data,
			new KinveyDownloadListParameters(KinveyDownloadCountType.All, 
				QueryBuilder.Instance.GetUserFavouriteEventKinveyQuery(user.Id)))
		{
			User = user;
		}

		public DownloadFavouriteEventsBackgroundTask(string eventId, string userId, 
			DynamicListData<FavouriteEventData> data): base(data,
			new KinveyDownloadListParameters(KinveyDownloadCountType.FirstPage, 
				QueryBuilder.Instance.GetIsFavouriteEventByUserKinveyQuery(userId, eventId)))
		{
		}
	}
}