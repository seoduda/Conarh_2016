using Conarh_2016.Application;
using Conarh_2016.Application.Domain;
using Core.Tasks;
using System;
using System.Collections.Generic;

namespace Conrarh_2016.Application.BackgroundTasks.GetData.Kinvey
{
    public enum KinveyDownloadCountType
    {
        All,
        FirstPage
    }

    public sealed class KinveyDownloadListParameters
    {
        public readonly KinveyDownloadCountType DownloadCountType;
        public readonly string Query;
        public readonly bool IsSaveToDb;

        public KinveyDownloadListParameters(KinveyDownloadCountType countType, string query, bool isSaveToDb = true)
        {
            DownloadCountType = countType;
            Query = query;
            IsSaveToDb = isSaveToDb;
        }
    }

    internal class DownloadListKinveyBackgroundTask<T, S> : OneShotBackgroundTask<List<T>>
        where T : UpdatedUniqueItem
        where S : RootListData<T>
    {
        public readonly KinveyDownloadListParameters TaskParameters;
        public readonly DynamicListData<T> DynamicList;

        public readonly List<T> Result = null;

        public DownloadListKinveyBackgroundTask(KinveyDownloadListParameters parameters)
        {
            TaskParameters = parameters;
            Result = new List<T>();
        }

        public DownloadListKinveyBackgroundTask(DynamicListData<T> data, KinveyDownloadListParameters parameters)
        {
            TaskParameters = parameters;
            DynamicList = data;
            Result = new List<T>();
        }

        public override List<T> Execute()
        {
            throw new NotImplementedException();
        }
        /*
private RootListData<T> DownloadPageData(int pageIndex)
{
   string newQuery = TaskParameters.Query;
   if (newQuery.Contains("?"))
       newQuery = string.Format("{0}&page={1}", newQuery, pageIndex);
   else
       newQuery = string.Format("{0}?page={1}", newQuery, pageIndex);

   return WebClient.GetObjectAsync<S>(newQuery).Result;
}

protected void UpdateData(RootListData<T> data)
{
   if (data != null)
   {
       Result.AddRange(data.Data);

       if (DynamicList != null)
           DynamicList.UpdateData(data.Data);

       if (TaskParameters.IsSaveToDb)
           DbClient.Instance.SaveData<T>(data.Data).ConfigureAwait(false);

       OnSaveData(data.Data);
   }
}

protected virtual void OnSaveData(List<T> data)
{
}

public override List<T> Execute()
{
   List<T> result = new List<T>();
   try
   {
       RootListData<T> listData = DownloadPageData(0);
       UpdateData(listData);

       if (listData != null)
       {
           if (listData.TotalPages > 1 && TaskParameters.DownloadCountType == DownloadCountType.All)
           {
               for (int pageIndex = listData.CurrentPage + 1; pageIndex < listData.TotalPages; pageIndex++)
                   UpdateData(DownloadPageData(pageIndex));
           }
       }

       return Result;
   }
   catch (AggregateException ex)
   {
       var serverException = ex.GetBaseException() as ServerException;

       if (serverException != null)
       {
           if (serverException.StatusCode == HttpStatusCode.NotFound)
               return result;
           else
               AppProvider.Log.WriteLine(LogChannel.Exception, ex);
       }
   }
   catch (Exception ex)
   {
       AppProvider.Log.WriteLine(LogChannel.Exception, ex);
   }

   return null;
}
*/
    }
}