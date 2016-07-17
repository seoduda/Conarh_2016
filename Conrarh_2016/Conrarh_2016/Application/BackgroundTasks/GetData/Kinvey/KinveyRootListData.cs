using Conarh_2016.Application.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Conarh_2016.Application.BackgroundTasks.GetData.Kinvey
{
    public class KinveyRootListData : RootListData
    {
    }

    public class KinveyRootListData<T> : KinveyRootListData where T : UniqueItem
    {
        private object result;

        [JsonProperty(KinveyRootListData.JsonKeys.Data)]
        public List<T> Data { get; set; }

        public KinveyRootListData(List<T> _data)
        {
            this.Data = _data;
            this.CurrentPage = 0;
            this.TotalPages = 1;
            this.CountPerPage = this.Data.Count;
        }

        public KinveyRootListData(object result)
        {
            this.Data = (List < T >) result;
        }

        public override string ToString()
        {
            return string.Format("[RootListData: CurrentPage={0} TotalPages={1} Data={2}]", CurrentPage, TotalPages, Data.Count);
        }
    }
}