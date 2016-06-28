using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Tools;
using System.Collections.Generic;
using Conarh_2016.Application.DataAccess;

namespace Conarh_2016.Application.BackgroundTasks
{
	public sealed class DownloadConnectRequestDataByUserBackgroundTask : DownloadListBackgroundTask<ConnectRequest, RootListData<ConnectRequest>>
	{
		public DownloadConnectRequestDataByUserBackgroundTask(DynamicListData<ConnectRequest> data, string query): base(data,
				new DownloadListParameters(DownloadCountType.All, query))
		{
		}

		protected override void OnSaveData(List<ConnectRequest> data)
		{
			foreach (ConnectRequest connectRequest in data) {
				AppModel.Instance.Users.UpdateData (new List<User>{ connectRequest.Requester, connectRequest.Responder });
				DbClient.Instance.SaveItemData<User> (connectRequest.Requester).ConfigureAwait(false);
				DbClient.Instance.SaveItemData<User> (connectRequest.Responder).ConfigureAwait(false);
			}
		}
	}
}