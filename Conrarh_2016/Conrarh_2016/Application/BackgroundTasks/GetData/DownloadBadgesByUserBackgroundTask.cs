
using Conarh_2016.Application.BackgroundTasks.GetData.Kinvey;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Tools;

namespace Conarh_2016.Application.BackgroundTasks
{
    //public sealed class DownloadBadgesByUserBackgroundTask : DownloadListBackgroundTask<BadgeAction,RootListData<BadgeAction>>
    public sealed class DownloadBadgesByUserBackgroundTask : DownloadListKinveyBackgroundTask<BadgeAction, KinveyRootListData<BadgeAction>>
    {
		public DownloadBadgesByUserBackgroundTask(DynamicListData<BadgeAction> dataModel, string userId):
		base(dataModel, new KinveyDownloadListParameters(KinveyDownloadCountType.All, 
			QueryBuilder.Instance.GetBadgesActionsByUserKinveyQuery(userId)))
		{
		}
	}
}