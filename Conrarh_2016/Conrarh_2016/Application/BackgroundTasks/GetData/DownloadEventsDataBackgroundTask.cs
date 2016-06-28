using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Tools;
using System.Collections.Generic;
using Conarh_2016.Application.DataAccess;

namespace Conarh_2016.Application.BackgroundTasks
{
	public sealed class DownloadEventsDataBackgroundTask : DownloadListBackgroundTask<EventData, RootListData<EventData>>
	{
		public DownloadEventsDataBackgroundTask(bool isFree, DynamicListData<EventData> data): base(data,
			new DownloadListParameters(DownloadCountType.All, QueryBuilder.Instance.GetEventsQuery(isFree)))
		{
		}

		protected override void OnSaveData(List<EventData> data)
		{
			foreach (EventData eventData in data) {
				AppModel.Instance.Speakers.UpdateData (eventData.Speechers);
				DbClient.Instance.SaveData<Speaker> (eventData.Speechers).ConfigureAwait (false);
			}
		}
	}
}