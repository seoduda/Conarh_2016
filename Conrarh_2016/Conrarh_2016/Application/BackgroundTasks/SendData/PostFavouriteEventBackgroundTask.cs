using Conarh_2016.Application.Tools;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.DataAccess;

namespace Conarh_2016.Application.BackgroundTasks
{
	public sealed class PostFavouriteEventBackgroundTask : PostDataBackgroundTask<FavouriteEventData>
	{
		public readonly User User;
		public readonly EventData EventData;
		public readonly DynamicListData<FavouriteEventData> ListData;

		public PostFavouriteEventBackgroundTask(User user, EventData eventData, DynamicListData<FavouriteEventData> listData):
		base(QueryBuilder.Instance.GetPostFavouriteEventKinveyQuery (), new FavouriteEventData (user.Id, eventData.Id),true)

		{
			ListData = listData;
			User = user;
			EventData = eventData;
		}

		protected override void OnResult (FavouriteEventData result)
		{
			if (result != null) {
				ListData.AddOne (result);
				DbClient.Instance.SaveItemData<FavouriteEventData> (result).ConfigureAwait (false);
			}
		}
	}
}