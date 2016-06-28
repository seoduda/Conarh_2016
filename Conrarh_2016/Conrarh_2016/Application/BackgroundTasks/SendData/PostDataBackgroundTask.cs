using System;
using Conarh_2016.Core;
using Conarh_2016.Core.Net;
using Conarh_2016.Core.Services;
using Newtonsoft.Json;
using Core.Tasks;

namespace Conarh_2016.Application.BackgroundTasks
{
	public class PostDataBackgroundTask<T> : OneShotBackgroundTask<T> where T:class, new()
	{
		public readonly T Data;
		public readonly string Query;
		public readonly bool IsPostQuery;

		public T Result;

		public PostDataBackgroundTask(string query, T data, bool isPost = true)
		{
			Data = data;
			Query = query;
			IsPostQuery = isPost;
		}

		public override T Execute ()
		{
			try
			{
				string serializedData = JsonConvert.SerializeObject (Data);
				string result = null;

				AppProvider.Log.WriteLine (LogChannel.All, Query + ":" + serializedData);

				try
				{
					if(IsPostQuery)
						result = WebClient.PostStringAsync(Query, serializedData).Result;
					else 
						result = WebClient.PutStringAsync(Query, serializedData).Result;
				}
				catch(Exception ex)
				{
					AppProvider.Log.WriteLine (LogChannel.All, ex);
				}

				AppProvider.Log.WriteLine (LogChannel.All, result);
				Result = JsonConvert.DeserializeObject<T>(result);

				OnResult(Result);
				return Result;
			}
			catch(Exception ex) 
			{
				AppProvider.Log.WriteLine (LogChannel.Exception, ex);
				Exception = ex;
			}

			return null;
		}

		protected virtual void OnResult (T result)
		{
			
		}
	}
}