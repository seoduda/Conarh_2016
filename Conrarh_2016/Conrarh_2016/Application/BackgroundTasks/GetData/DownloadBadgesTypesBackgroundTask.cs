using Conarh_2016.Application.BackgroundTasks.GetData.Kinvey;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Tools;
using System.Collections.Generic;

namespace Conarh_2016.Application.BackgroundTasks
{
	//public sealed class DownloadBadgesTypesBackgroundTask : DownloadListBackgroundTask<BadgeType, RootListData<BadgeType>>
    public sealed class DownloadBadgesTypesBackgroundTask : DownloadListKinveyBackgroundTask<BadgeType, KinveyRootListData<BadgeType>>
    {
		public DownloadBadgesTypesBackgroundTask(): base(new KinveyDownloadListParameters(KinveyDownloadCountType.All,
			QueryBuilder.Instance.GetBadgeTypesKinveyQuery()))
		{
		}

		public override List<BadgeType> Execute ()
		{
			if (AppModel.Instance.BadgeTypes.Items.Count > 0 )
				return AppModel.Instance.BadgeTypes.Items;

			var result = base.Execute ();
			if (result != null)
				AppModel.Instance.BadgeTypes.UpdateData (result);

			return result;
		}
	}
}