using Conarh_2016.Application.BackgroundTasks.GetData.Kinvey;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Tools;
using System.Collections.Generic;

namespace Conarh_2016.Application.BackgroundTasks
{
    //public sealed class DownloadSponsorTypesBackgroundTask : DownloadListBackgroundTask<SponsorType, RootListData<SponsorType>>
    public sealed class DownloadSponsorTypesBackgroundTask : DownloadListKinveyBackgroundTask<SponsorType, KinveyRootListData<SponsorType>>
    {
        public DownloadSponsorTypesBackgroundTask() : base(new KinveyDownloadListParameters(KinveyDownloadCountType.All,
            QueryBuilder.Instance.GetSponsorTypesKinveyQuery()))
        {
        }

        public override List<SponsorType> Execute()
        {
            /*

            if (AppModel.Instance.SponsorTypes.Items.Count > 0)
                return new List<SponsorType>();
                */

            List<SponsorType> result;
            result = base.Execute();


            //List<SponsorType> result = base.Execute();

            if (result != null)
                AppModel.Instance.SponsorTypes.UpdateData(result);

            return result;
        }

    }

    /*
    private List<SponsorType> getKinveySponsorType()
    {
        List<SponsorType> result = new List<SponsorType>();
        Client kinveyClient = new Client.Builder(Config.KinveyKey, Config.KinveySecret).build();

        KinveyXamarin.User activeUser = kinveyClient.User().LoginBlocking().Execute();

        AsyncAppData<SponsorType> mysts = kinveyClient.AppData<SponsorType>("sponsorType", typeof(SponsorType));
        try
        {
            SponsorType[] sts = mysts.GetBlocking().Execute();
            foreach (SponsorType st in sts)
            {
                result.Add(st);
            }
        }
        catch (Exception e)
        {
            AppProvider.Log.WriteLine(LogChannel.Exception, e.Message);
        }

        return result;
    }


}

public class DownloadSponsorTypesBackgroundTask : DownloadListBackgroundTask<SponsorType, RootListData<SponsorType>>
{

    public DownloadSponsorTypesBackgroundTask(): base(new DownloadListParameters(DownloadCountType.All,
        QueryBuilder.Instance.GetSponsorTypesQuery()))
    {
    }
    /*
    public DownloadSponsorTypesBackgroundTask()
    {

    }


    public override List<SponsorType> Execute ()
    {

        if (AppModel.Instance.SponsorTypes.Items.Count > 0)
            return new List<SponsorType>();


        //List<SponsorType> result;
        //result = getKinveySponsorType();
        //result = base.Execute();
        List<SponsorType> result = base.Execute ();

        //List<SponsorType> result = base.Execute ();

        if (result != null)
            AppModel.Instance.SponsorTypes.UpdateData (result);


        return result;
    }

}
*/
}