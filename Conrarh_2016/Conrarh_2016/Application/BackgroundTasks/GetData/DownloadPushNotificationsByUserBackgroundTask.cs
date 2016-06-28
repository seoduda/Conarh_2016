using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Tools;

namespace Conarh_2016.Application.BackgroundTasks
{
	public sealed class DownloadPushNotificationsByUserBackgroundTask : DownloadListBackgroundTask<PushNotificationData,RootListData<PushNotificationData>>
	{
		public DownloadPushNotificationsByUserBackgroundTask(string userId, string platform):
		base(new DownloadListParameters(DownloadCountType.FirstPage, QueryBuilder.Instance.GetPushNotificationsByUserQuery (userId, platform)))
		{
		}

		public DownloadPushNotificationsByUserBackgroundTask(string query):
		base(new DownloadListParameters(DownloadCountType.FirstPage, query))
		{
		}
	}
}