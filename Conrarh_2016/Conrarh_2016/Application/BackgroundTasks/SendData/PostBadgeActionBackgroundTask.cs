using Conarh_2016.Application.Domain.PostData;
using Conarh_2016.Application.Tools;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.DataAccess;

namespace Conarh_2016.Application.BackgroundTasks
{
	public sealed class PostBadgeActionBackgroundTask : PostDataBackgroundTask<BadgeAction>
	{
		public readonly DynamicListData<BadgeAction> ListData;

		public PostBadgeActionBackgroundTask(AppBadgeType badgeType, string userId, DynamicListData<BadgeAction> listData):
		base(QueryBuilder.Instance.GetPostBadgesActionKinveyQuery(), new BadgeAction (userId, AppResources.GetBadgeIdByType(badgeType)), true)
		{
			ListData = listData;
		}

		protected override void OnResult (BadgeAction result)
		{
			if (result != null) {
				
				if(ListData != null)
					ListData.AddOne (result);
				
				DbClient.Instance.SaveItemData<BadgeAction> (result).ConfigureAwait (false);
			}
		}
	}
}