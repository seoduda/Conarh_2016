using System;
using Conarh_2016.Application.Domain.PostData;
using Conarh_2016.Application.Tools;
using Conarh_2016.Core;
using Conarh_2016.Core.Net;
using Conarh_2016.Core.Services;
using Newtonsoft.Json;
using Core.Tasks;

namespace Conarh_2016.Application.BackgroundTasks
{
	public sealed class RequestConnectionBackgroundTask : OneShotBackgroundTask<RequestConnectionData>
	{
		public readonly RequestConnectionData RequestData;
		public readonly RequestConnectionData AcceptData;
		public readonly string RequestId;

		public RequestConnectionBackgroundTask(RequestConnectionData data, bool isAcceptData, string requestId)
		{
			if (!isAcceptData)
				RequestData = data;
			else
				AcceptData = data;

			RequestId = requestId;
		}

		public override RequestConnectionData Execute ()
		{
			try
			{
				string result = null;
				if(RequestData != null)
				{
					string serializedData = JsonConvert.SerializeObject (RequestData);
					result = WebClient.PostStringAsync(QueryBuilder.Instance.GetPostRequestConnectionQuery (), serializedData).Result;
				}
				else
				{
					string serializedData = JsonConvert.SerializeObject (AcceptData);

					if(string.IsNullOrEmpty(RequestId))
						result = WebClient.PostStringAsync(QueryBuilder.Instance.GetPostRequestConnectionQuery (), serializedData).Result;
					else
					{
						string query = QueryBuilder.Instance.GetPostRequestConnectionQuery(RequestId);
						AppProvider.Log.WriteLine(LogChannel.All, query + " " + serializedData);
						result = WebClient.PutStringAsync(query, serializedData).Result;
					}
				}
					
				AppProvider.Log.WriteLine (LogChannel.All, result);
				return JsonConvert.DeserializeObject<RequestConnectionData>(result);
			}
			catch(Exception ex) 
			{
				AppProvider.Log.WriteLine (LogChannel.Exception, ex);
				Exception = ex;
			}

			return null;
		}
	}
}