using Conarh_2016.Application.BackgroundTasks.GetData.Kinvey;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Tools;
using System;
using System.Collections.Generic;

namespace Conarh_2016.Application.BackgroundTasks.GetData
{
    class DownloadSpeakersBackgroundTask : Kinvey.DownloadListKinveyBackgroundTask<Speaker, KinveyRootListData<Speaker>>
    {
        public DownloadSpeakersBackgroundTask() : base(new KinveyDownloadListParameters(KinveyDownloadCountType.All,
            QueryBuilder.Instance.GetPostSpeakersKinveyQuery()))
        {
        }

        public override List<Speaker> Execute()
        {

            if (AppModel.Instance.Speakers.Items.Count > 0)
                return new List<Speaker>();

            List<Speaker> result;
            result = base.Execute();


            if (result != null)
                AppModel.Instance.Speakers.UpdateData(result);

            return result;
        }


    }
}
