using System;
using Conarh_2016.Core;
using Conarh_2016.Core.Net;
using Conarh_2016.Core.Services;
using Newtonsoft.Json;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.DataAccess;
using Core.Tasks;

namespace Conarh_2016.Application.BackgroundTasks
{
	public class DeleteDataBackgroundTask<T> : OneShotBackgroundTask<UniqueItem> where T:UpdatedUniqueItem
	{
		public readonly string Query;
		public readonly T Data;
		public UniqueItem Result;
		public readonly DynamicListData<T> ListData;

		public DeleteDataBackgroundTask(DynamicListData<T> listData, T data, string query)
		{
			Data = data;
			Query = query;
			ListData = listData;
		}

		public override UniqueItem Execute ()
		{
			try
			{
				string result = KinveyWebClient.DeleteStringAsync(Query).Result;

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

		protected virtual void OnResult (UniqueItem result)
		{
			if (result != null) {
				DbClient.Instance.DeleteItemData<T> (Data).ConfigureAwait(false);
				if (ListData != null)
					ListData.DeleteOne (Data);
			}
		}
	}
}