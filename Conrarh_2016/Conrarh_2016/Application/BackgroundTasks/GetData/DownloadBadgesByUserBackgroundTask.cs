using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Tools;

namespace Conarh_2016.Application.BackgroundTasks
{
	public sealed class DownloadBadgesByUserBackgroundTask : DownloadListBackgroundTask<BadgeAction,RootListData<BadgeAction>>
	{
		public DownloadBadgesByUserBackgroundTask(DynamicListData<BadgeAction> dataModel, string userId):
		base(dataModel, new DownloadListParameters(DownloadCountType.All, 
			QueryBuilder.Instance.GetBadgesActionsByUserQuery (userId)))
		{
		}
	}
}