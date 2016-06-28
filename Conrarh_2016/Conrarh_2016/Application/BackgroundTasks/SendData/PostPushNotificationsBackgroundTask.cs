using Conarh_2016.Application.Domain.PostData;
using Conarh_2016.Application.Tools;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.DataAccess;

namespace Conarh_2016.Application.BackgroundTasks
{
	public sealed class PostPushNotificationsBackgroundTask : PostDataBackgroundTask<PushNotificationData>
	{
		public readonly PushNotificationData PushData;
		public PostPushNotificationsBackgroundTask(PushNotificationData data, bool isPost, string query): 
		base(query, data, isPost)
		{
			PushData = data;
		}

		protected override void OnResult (PushNotificationData result)
		{
			PushData.Id = result.Id;
			DbClient.Instance.SaveData<PushNotificationData> (PushData).ConfigureAwait(false);
		}
	}
}