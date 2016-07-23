using Conarh_2016.Application.BackgroundTasks.GetData.Kinvey;
using Conarh_2016.Application.DataAccess;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Tools;
using Conarh_2016.Core.Net;
using System.Collections.Generic;

namespace Conarh_2016.Application.BackgroundTasks
{
    //public sealed class DownloadConnectRequestDataByUserBackgroundTask : DownloadListBackgroundTask<ConnectRequest, RootListData<ConnectRequest>>
    public sealed class DownloadConnectRequestDataByUserBackgroundTask : DownloadListKinveyBackgroundTask<ConnectRequest, KinveyRootListData<ConnectRequest>>
    {
        public DownloadConnectRequestDataByUserBackgroundTask(DynamicListData<ConnectRequest> data, string query) : base(data,
                new KinveyDownloadListParameters(KinveyDownloadCountType.All, query))
        {
        }

        protected override void OnSaveData(List<ConnectRequest> data)
        {
            foreach (ConnectRequest connectRequest in data)
            {
                User Requester = AppModel.Instance.Users.Find(connectRequest.RequesterId);
                User Responder = AppModel.Instance.Users.Find(connectRequest.ResponderId);

                AppModel.Instance.Users.UpdateData(new List<User> { Requester, Responder });
                DbClient.Instance.SaveItemData<User>(Requester).ConfigureAwait(false);
                DbClient.Instance.SaveItemData<User>(Responder).ConfigureAwait(false);
                /* Todo clean
                AppModel.Instance.Users.UpdateData(new List<User> { connectRequest.Requester, connectRequest.Responder });
                DbClient.Instance.SaveItemData<User>(connectRequest.Requester).ConfigureAwait(false);
                DbClient.Instance.SaveItemData<User>(connectRequest.Responder).ConfigureAwait(false);
                */

            }
        }
    }
}