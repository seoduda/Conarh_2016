using Conarh_2016.Application.DataAccess;
using Conarh_2016.Application.Domain;
using Conarh_2016.Core;
using Conarh_2016.Core.Net;
using Conarh_2016.Core.Services;
using Core.Tasks;
using System;
using System.Collections.Generic;

namespace Conarh_2016.Application.BackgroundTasks.GetData.Kinvey
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

    public class DownloadListKinveyBackgroundTask<T, S> : OneShotBackgroundTask<List<T>>
        where T : UpdatedUniqueItem
        where S : KinveyRootListData<T>
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

        private KinveyRootListData<T> DownloadPageData(int pageIndex)
        {
            string newQuery = TaskParameters.Query;
            //newQuery = QueryBuilder.Instance.GetSponsorTypesKinveyQuery();
            /*
            if (newQuery.Contains("?"))
                newQuery = string.Format("{0}&page={1}", newQuery, pageIndex);
            else
                newQuery = string.Format("{0}?page={1}", newQuery, pageIndex);
             */
            //String s = KinveyWebClient.GetStringAsync()
            List<T> _data = KinveyWebClient.GetObjectAsync<List<T>>(newQuery).Result;
            //_data = setIds(_data);
            KinveyRootListData<T> krld = new KinveyRootListData<T>(_data);

            return krld;
        }

        protected void UpdateData(KinveyRootListData<T> data)
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
                KinveyRootListData<T> listData = DownloadPageData(0);
                UpdateData(listData);

                if (listData != null)
                {
                    if (listData.TotalPages > 1 && TaskParameters.DownloadCountType == KinveyDownloadCountType.All)
                    {
                        for (int pageIndex = listData.CurrentPage + 1; pageIndex < listData.TotalPages; pageIndex++)
                            UpdateData(DownloadPageData(pageIndex));
                    }
                }

                return Result;
            }
            catch (Exception ex)
            {
                AppProvider.Log.WriteLine(LogChannel.Exception, ex);
            }
            return null;
            /*
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
            */
        }
         /* TODO apagar setEventsIds
        private List<T> setIds(List<T> result)
        {
            List<T> objList = new List<T>();
            foreach (T obj in result)
            {
                obj.Id = obj.Xid;
                objList.Add(obj);
            }
            return objList;
        }
        */

    }
}