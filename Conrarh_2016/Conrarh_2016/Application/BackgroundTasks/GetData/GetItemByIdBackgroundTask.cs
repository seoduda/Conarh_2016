using System;
using Conarh_2016.Application.Domain;
using Conarh_2016.Core;
using Conarh_2016.Core.Net;
using Conarh_2016.Core.Services;
using Conarh_2016.Application.DataAccess;
using Core.Tasks;

namespace Conarh_2016.Application.BackgroundTasks
{
	public class GetItemByIdBackgroundTask<T> : OneShotBackgroundTask<T> 
		where T:UpdatedUniqueItem
	{
		public readonly DynamicListData<T> DynamicList;
		public readonly string Query;
		public T Result;

		public GetItemByIdBackgroundTask(string query, DynamicListData<T> data)
		{
			Query = query;
			DynamicList = data;
		}
			
		public override T Execute ()
		{
			try
			{
				Result = KinveyWebClient.GetObjectAsync<T>(Query).Result;

				if(DynamicList != null)
					DynamicList.AddOne(Result);

				DbClient.Instance.SaveItemData<T> (Result).ConfigureAwait (false);
				
				return Result;
			}
			catch(Exception ex)
			{
				AppProvider.Log.WriteLine (LogChannel.Exception, ex);
			}

			return null;
		}
	}
}