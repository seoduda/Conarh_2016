using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Tools;

namespace Conarh_2016.Application.BackgroundTasks
{
	public sealed class DownloadEventUserVotesBackgroundTask : DownloadListBackgroundTask<UserVoteData,RootListData<UserVoteData>>
	{
		public DownloadEventUserVotesBackgroundTask(string eventId, string userId):
		base(AppModel.Instance.CurrentUser.VoteData, new DownloadListParameters(DownloadCountType.All, 
			QueryBuilder.Instance.GetEventVotesByUserQuery (eventId, userId)))
		{
		}
	}
}