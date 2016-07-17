using Conarh_2016.Application.BackgroundTasks.GetData.Kinvey;
using Conarh_2016.Application.DataAccess;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Tools;
using Conarh_2016.Core.Net;
using System.Collections.Generic;

namespace Conarh_2016.Application.BackgroundTasks
{
    //public sealed class DownloadEventsDataBackgroundTask : DownloadListBackgroundTask<EventData, RootListData<EventData>>
    public sealed class DownloadEventsDataBackgroundTask : DownloadListKinveyBackgroundTask<EventData, KinveyRootListData<EventData>>
    {
        private bool _isFree;

        public DownloadEventsDataBackgroundTask(bool isFree, DynamicListData<EventData> data) : base(data,
            new KinveyDownloadListParameters(KinveyDownloadCountType.All, QueryBuilder.Instance.GetEventsKinveyQuery(isFree)))
        {
            _isFree = isFree;
        }

        public override List<EventData> Execute()
        {
            /*
                if (AppModel.Instance.SponsorTypes.Items.Count > 0)
                    return new List<Exhibitor>();
              */
            List<EventData> result;

            //result = base.Execute();
            result = getEventList();

            if (result != null)
                if (_isFree)
                {
                    AppModel.Instance.FreeEvents.ClearData();
                    AppModel.Instance.FreeEvents.UpdateData(result);
                }
                else
                {
                    AppModel.Instance.PayedEvents.ClearData();
                    AppModel.Instance.PayedEvents.UpdateData(result);
                }

            return result;
        }

        protected override void OnSaveData(List<EventData> data)
        {
            foreach (EventData eventData in data)
            {
                AppModel.Instance.Speakers.UpdateData(eventData.Speechers);
                DbClient.Instance.SaveData<Speaker>(eventData.Speechers).ConfigureAwait(false);
            }
        }

        private List<EventData> getEventList()
        {
            string newQuery = TaskParameters.Query;
            List<EventData> _data = KinveyWebClient.GetObjectAsync<List<EventData>>(newQuery).Result;
            //_data = setEventsIds(_data);
            KinveyRootListData<EventData> krld = new KinveyRootListData<EventData>(_data);

            return _data;
        }

        /* TODO apagar setEventsIds
        private List<EventData> setEventsIds(List<EventData> result)
        {
            List<EventData> objList = new List<EventData>();
            foreach (EventData obj in result)
            {
                obj.Id = obj.Xid;
                foreach (Speaker spk in obj.Speechers)
                {
                    spk.Id = spk.Xid;
                }
                objList.Add(obj);
            }
            return objList;
        }
        */
    }
}