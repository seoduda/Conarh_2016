using Conarh_2016.Application.Tools;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.DataAccess;
using System.Collections.Generic;

namespace Conarh_2016.Application.BackgroundTasks
{
	public sealed class PostEventVoteBackgroundTask : PostDataBackgroundTask<UserVoteData>
	{
		public readonly DynamicListData<UserVoteData> ListData;
		public readonly UserVoteData UserVoteData;

		public PostEventVoteBackgroundTask(UserVoteData voteData, DynamicListData<UserVoteData> listData):
		base(GetQuery(voteData), GetPutData(voteData), string.IsNullOrEmpty(voteData.Id))
		{
			ListData = listData;
			UserVoteData = voteData;
		}

		protected override void OnResult (UserVoteData result)
		{
			if (result != null) 
			{
				if (string.IsNullOrEmpty (result.Id))
					result.Id = UserVoteData.Id;
				else if (string.IsNullOrEmpty (UserVoteData.Id))
					UserVoteData.Id = result.Id;
				
				ListData.UpdateData (new List<UserVoteData>{result});
				DbClient.Instance.SaveItemData<UserVoteData> (IsPostQuery ? result : UserVoteData).ConfigureAwait (false);
			}
		}

		public static string GetQuery(UserVoteData voteData)
		{
			if (string.IsNullOrEmpty(voteData.Id))
				return QueryBuilder.Instance.GetPostUserVotesQuery ();
			
			return QueryBuilder.Instance.GetPostUserVotesQuery (voteData.Id);
		}

		public static UserVoteData GetPutData(UserVoteData voteData)
		{
			return new UserVoteData { 
				Vote = voteData.Vote,
				UserVoteType = voteData.UserVoteType,
				Subject = voteData.Subject,
				User = voteData.User,
				Event = voteData.Event
			};
		}
	}
}