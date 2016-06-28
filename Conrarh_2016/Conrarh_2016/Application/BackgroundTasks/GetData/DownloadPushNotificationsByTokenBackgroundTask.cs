using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Tools;

namespace Conarh_2016.Application.BackgroundTasks
{
	public sealed class DownloadPushNotificationsByTokenBackgroundTask : DownloadListBackgroundTask<PushNotificationData,RootListData<PushNotificationData>>
	{
		public DownloadPushNotificationsByTokenBackgroundTask(string token, string platform):
		base(new DownloadListParameters(DownloadCountType.All, QueryBuilder.Instance.GetPushNotificationsByTokenQuery (token, platform), false))
		{
		}
	}
}