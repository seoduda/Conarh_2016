using Conarh_2016.Application.BackgroundTasks.GetData.Kinvey;
using Conarh_2016.Application.DataAccess;
using Conarh_2016.Application.Domain;
using Conrarh_2016.Core.DataAccess.Local;
using System.Collections.Generic;

namespace Conarh_2016.Application.BackgroundTasks
{
    //public sealed class DownloadExhibitorsBackgroundTask : DownloadListBackgroundTask<Exhibitor, RootListData<Exhibitor>>
    public sealed class DownloadExhibitorsBackgroundTask : DownloadListKinveyBackgroundTask<Exhibitor, KinveyRootListData<Exhibitor>>
    {
        public readonly DynamicListData<Exhibitor> DList;

        public DownloadExhibitorsBackgroundTask(KinveyDownloadListParameters parameters) : base(parameters)
        {
        }

        public DownloadExhibitorsBackgroundTask(DynamicListData<Exhibitor> data, KinveyDownloadListParameters parameters) : base(data, parameters)
        {
            DList = data;
        }

        
        
        public override List<Exhibitor> Execute()
        {
        /*
            if (AppModel.Instance.SponsorTypes.Items.Count > 0)
                return new List<Exhibitor>();
          */
            List<Exhibitor> result;
            //result = LocalData.getLocalExhibitorList();
            result = base.Execute();
                
                //var x = AppModel.Instance.Exhibitors.Items.Count;
               // DbClient.Instance.SaveData<Exhibitor>(result).ConfigureAwait(false);
            if (result != null)
                AppModel.Instance.Exhibitors.UpdateData(result);

            return result;
        }





        /*
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
                        AppProvider.Log.WriteLine(LogChannel.Exception, e.Message);
                    }

                    return result;
                }
                */
    }
}