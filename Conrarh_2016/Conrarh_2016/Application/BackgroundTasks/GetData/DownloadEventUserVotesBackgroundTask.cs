using Conarh_2016.Application.BackgroundTasks.GetData.Kinvey;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Tools;

namespace Conarh_2016.Application.BackgroundTasks
{
    //public sealed class DownloadEventUserVotesBackgroundTask : DownloadListBackgroundTask<UserVoteData,RootListData<UserVoteData>>
    public sealed class DownloadEventUserVotesBackgroundTask : GetData.Kinvey.DownloadListKinveyBackgroundTask<UserVoteData, KinveyRootListData<UserVoteData>>
    {
		public DownloadEventUserVotesBackgroundTask(string eventId, string userId):
		base(AppModel.Instance.CurrentUser.VoteData, new KinveyDownloadListParameters(KinveyDownloadCountType.All, 
			QueryBuilder.Instance.GetEventVotesByUserKinveyQuery(eventId, userId)))
		{
		}
	}
}