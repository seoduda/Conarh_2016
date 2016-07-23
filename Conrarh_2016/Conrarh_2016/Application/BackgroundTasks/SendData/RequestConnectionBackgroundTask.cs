using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Domain.PostData;
using Conarh_2016.Application.Tools;
using Conarh_2016.Core;
using Conarh_2016.Core.Net;
using Conarh_2016.Core.Services;
using Core.Tasks;
using Newtonsoft.Json;
using System;

namespace Conarh_2016.Application.BackgroundTasks
{
    public sealed class RequestConnectionBackgroundTask : OneShotBackgroundTask<RequestConnectionData>
    {
        public readonly RequestConnectionData RequestData;
        public readonly RequestConnectionData AcceptData;
        //private ConnectRequest ConnReq;

        public readonly string RequestId;

        public RequestConnectionBackgroundTask(RequestConnectionData data, bool isAcceptData, string requestId)
        {
          
            if (!isAcceptData)
                RequestData = data;
            else
                AcceptData = data;

            RequestId = requestId;
        }

        public override RequestConnectionData Execute()
        {
            try
            {
                string result = null;
                if (RequestData != null)
                {
                    //     string serializedData = JsonConvert.SerializeObject(ConnReq);
                    /*
                    var settings = new JsonSerializerSettings
                    {
                        Error = (sender, args) =>
                        {
                            if (System.Diagnostics.Debugger.IsAttached)
                            {
                                System.Diagnostics.Debugger.Break();
                            }
                        }
                    };*/

                    string serializedData = JsonConvert.SerializeObject(RequestData);

                    result = KinveyWebClient.PostStringAsync(QueryBuilder.Instance.GetPostRequestConnectionKinveyQuery(), serializedData).Result;
                }
                else
                {
                    string serializedData = JsonConvert.SerializeObject(AcceptData);

                    if (string.IsNullOrEmpty(RequestId))
                        result = KinveyWebClient.PostStringAsync(QueryBuilder.Instance.GetPostRequestConnectionKinveyQuery(), serializedData).Result;
                    else
                    {
                        string query = QueryBuilder.Instance.GetPostRequestConnectionKinveyQuery(RequestId);
                        AppProvider.Log.WriteLine(LogChannel.All, query + " " + serializedData);
                        result = KinveyWebClient.PutStringAsync(query, serializedData).Result;
                    }
                }
                ConnectRequest ResultConnReq = JsonConvert.DeserializeObject<ConnectRequest>(result);
                RequestConnectionData resultRCD = JsonConvert.DeserializeObject<RequestConnectionData>(result);
                AppProvider.Log.WriteLine(LogChannel.All, result);
                return resultRCD;
            }
            catch (Exception ex)
            {
                AppProvider.Log.WriteLine(LogChannel.Exception, ex);
                Exception = ex;
            }

            return null;
        }
    }
}