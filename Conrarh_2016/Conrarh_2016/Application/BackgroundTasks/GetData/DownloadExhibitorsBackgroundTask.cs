using Conarh_2016.Application.DataAccess;
using Conarh_2016.Application.Domain;
using Conrarh_2016.Core.DataAccess;
using KinveyXamarin;
using System;
using System.Collections.Generic;

namespace Conarh_2016.Application.BackgroundTasks
{
    public sealed class DownloadExhibitorsBackgroundTask : DownloadListBackgroundTask<Exhibitor, RootListData<Exhibitor>>
    {
        public readonly DynamicListData<Exhibitor> DList;

        public DownloadExhibitorsBackgroundTask(DownloadListParameters parameters) : base(parameters)
        {
        }

        public DownloadExhibitorsBackgroundTask(DynamicListData<Exhibitor> data, DownloadListParameters parameters) : base(data, parameters)
        {
            DList = data;
        }

        public override List<Exhibitor> Execute()
        {
            List<Exhibitor> result;
            result = getKinveyExhibitors();
            if (DList != null)
            {
                DList.ClearData();
                foreach (Exhibitor xb in result)
                {
                    DList.AddOne(xb);
                }
                //DList.UpdateData(result);
                DbClient.Instance.SaveData<Exhibitor>(result).ConfigureAwait(false);
            }

            return result;
        }

        private List<Exhibitor> getKinveyExhibitors()
        {
            List<Exhibitor> result = new List<Exhibitor>();
            Client kinveyClient = new Client.Builder(Config.KinveyKey, Config.KinveySecret).build();

            KinveyXamarin.User activeUser = kinveyClient.User().LoginBlocking().Execute();

            AsyncAppData<Exhibitor> myexhibitors = kinveyClient.AppData<Exhibitor>("exhibitors", typeof(Exhibitor));
            try
            {
                Exhibitor[] xbitosors = myexhibitors.GetBlocking().Execute();
                foreach (Exhibitor xb in xbitosors)
                {
                    result.Add(xb);
                }
            }
            catch (Exception e)
            {
                //ops
            }

            return result;
        }
    }
}