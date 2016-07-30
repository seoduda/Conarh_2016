using Conarh_2016.Application.BackgroundTasks.GetData.Kinvey;
using Conarh_2016.Application.DataAccess;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Tools;
using Conarh_2016.Core.Net;
using System.Collections.Generic;

namespace Conarh_2016.Application.BackgroundTasks
{
    //public sealed class DownloadConnectRequestDataByUserBackgroundTask : DownloadListBackgroundTask<ConnectRequest, RootListData<ConnectRequest>>
    public sealed class DownloadUserConnectRequestDataBackgroundTask : GetData.Kinvey.DownloadListKinveyBackgroundTask<ConnectRequest, KinveyRootListData<ConnectRequest>>
    {
        //private string _usrId;

        public DownloadUserConnectRequestDataBackgroundTask(DynamicListData<ConnectRequest> data, string query) : base(data,
                new KinveyDownloadListParameters(KinveyDownloadCountType.All, query))
        {
          //  this._usrId = userId;
        }
        /*
        public override List<ConnectRequest> Execute()
        {

            //List<ConnectRequest> result = base.Execute();
            //string _query = QueryBuilder.Instance.GetConnectRequestedByUserKinveyQuery(_usrId);
            List<ConnectRequest> result = getUserConnectRequests(query);
            /*
            _query = QueryBuilder.Instance.GetConnectRequestedToUserKinveyQuery(_usrId);
            result.AddRange(getUserConnectRequests(_query));

            result.Sort(delegate (ConnectRequest x, ConnectRequest y)
            {
                // Sort by UpdatedAtTime in descending order
                int a = y.UpdatedAtTime.CompareTo(x.UpdatedAtTime);

                // Both player has the same UpdatedAtTime.
                // Sort by Accepted in descending order
                if (a == 0)
                    a = y.Accepted.CompareTo(x.Accepted);

                return a;
            });
            

            if (result != null)
                AppModel.Instance.Requests.UpdateData(result);

            return result;
        }
        */
        protected override void OnSaveData(List<ConnectRequest> data)
        {
            foreach (ConnectRequest connectRequest in data)
            {
                User reqs = AppModel.Instance.Users.Find(connectRequest.RequesterId);
                User resp = AppModel.Instance.Users.Find(connectRequest.ResponderId);
                AppModel.Instance.Users.UpdateData(new List<User> {
                    reqs,resp
                });
                DbClient.Instance.SaveItemData<User>(reqs).ConfigureAwait(false);
                DbClient.Instance.SaveItemData<User>(resp).ConfigureAwait(false);
                

                /* Todo validar
                AppModel.Instance.Users.UpdateData(new List<User> { connectRequest.Requester, connectRequest.Responder });
                DbClient.Instance.SaveItemData<User>(connectRequest.Requester).ConfigureAwait(false);
                DbClient.Instance.SaveItemData<User>(connectRequest.Responder).ConfigureAwait(false);
                */
            }
        }
        /*
        private List<ConnectRequest> getUserConnectRequests(string _query)
        {
            ///string result = await GetStringAsync(requestUri, JsonMimeType, timeout, cancellationToken).ConfigureAwait(false) ?? String.Empty;
            List<ConnectRequest> _data =  KinveyWebClient.GetObjectAsync<List<ConnectRequest>>(_query).Result;
            //_data = setEventsIds(_data);
            KinveyRootListData<ConnectRequest> krld = new KinveyRootListData<ConnectRequest>(_data);

            return _data;
        }
        */
    }
}