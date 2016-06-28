using System;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Domain.PostData;
using Conarh_2016.Application.Tools;
using Conarh_2016.Core;
using Conarh_2016.Core.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using Conarh_2016.Application.DataAccess;
using Core.Tasks;

namespace Conarh_2016.Application.BackgroundTasks
{
	public sealed class DeleteOldPushNotificationsBackgroundTask : OneShotBackgroundTask<bool>
	{
		public readonly List<PushNotificationData> PushData;

		public DeleteOldPushNotificationsBackgroundTask(List<PushNotificationData> pushData)
		{
			PushData = pushData;
		}

		public override bool Execute ()
		{
			foreach (PushNotificationData data in PushData) {
				try
				{
					string query = QueryBuilder.Instance.GetDeletePushNotificationsQuery(data.Id);
					string result = WebClient.DeleteStringAsync(query).Result;
					DbClient.Instance.DeleteItemData<PushNotificationData>(data).ConfigureAwait(false);
				}
				catch
				{
				}
			}

			return true;
		}
	}
}