using System.Linq;
using System;
using Conarh_2016.Core;

namespace Conarh_2016.Application.Tools
{
	public sealed class QueryBuilder: Singletone<QueryBuilder>
	{
		internal string ApiUrl = "http://conarh-homol.goldarkapi.com";

		public static string CombinePath(params string[] segments)
		{
			return segments.Aggregate((total, next) => String.Format("{0}/{1}", total.TrimEnd('/'), next));
		}

		public string GetQuery(params string[] parts)
		{
			return CombinePath(ApiUrl, CombinePath(parts));
		}
			
		public string GetEventsQuery(bool isFree)
		{
			return GetQuery(string.Format("events?free_attending={0}&per_page=8&order_by=date:asc", isFree.ToString().ToLower()));
		}
			
		public string GetExhibitorsQuery()
		{
			return GetQuery("exhibitors?per_page=5");
		}

		public string GetSponsorTypesQuery()
		{
			return GetQuery("sponsor_types?order_by=type:asc");
		}

		public string GetBadgeTypesQuery()
		{
			return GetQuery("badges_list");
		}
	
		public string GetUsersQuery()
		{
			return GetQuery ("users");
		}

		public string GetUsersSortByNameQuery()
		{
			return GetQuery ("users?sort_by=name:asc");
		}

		public string GetLoginUserSessionQuery()
		{
			return GetQuery ("sessions");
		}

		public string GetWallPostsQuery()
		{
			return GetQuery ("wall");
		}

		public string GetWallPostByIdQuery (string wallPostId)
		{
			return GetQuery (string.Format("wall/{0}", wallPostId));
		}

		public string GetWallPostsLikesQuery(string wallPostId)
		{
			return GetQuery (string.Format("wall_likes?post={0}", wallPostId));
		}

		public string GetWallPostsLikesQuery(string wallPostId, string userId)
		{
			return GetQuery (string.Format("wall_likes?post={0}&user={1}", wallPostId, userId));
		}

		public string GetPostUserVotesQuery()
		{
			return GetQuery ("user_votes");
		}

		public string GetPostUserVotesQuery(string voteId)
		{
			return GetQuery (string.Format("user_votes/{0}", voteId));
		}

		public string GetUserByUsernameQuery(string username)
		{
			return GetQuery (string.Format("users?username={0}", username));
		}

		public string GetPostQuestionOnEventQuery()
		{
			return GetQuery ("questions_events");
		}

		public string GetPostWallPostLikeQuery()
		{
			return GetQuery ("wall_likes");
		}

		public string GetPostPushNotificationsQuery()
		{
			return GetQuery ("push/devices");
		}

		public string GetPutPushNotificationsQuery(string id)
		{
			return GetQuery (string.Format("push/devices/{0}", id));
		}

		public string GetPushNotificationsByUserQuery(string userid, string platform)
		{
			return GetQuery (string.Format("push/devices?user_id={0}&platform={1}", userid, platform));
		}

		public string GetPushNotificationsByTokenQuery(string token, string platform)
		{
			return GetQuery (string.Format("push/devices?token={0}&platform={1}", token, platform));
		}

		public string GetDeletePushNotificationsQuery(string id)
		{
			return GetQuery (string.Format("push/devices/{0}", id));
		}


		public string GetPostWallPostImageQuery(string wallPostId)
		{
			return GetQuery (string.Format("wall/{0}/image", wallPostId));
		}

		public string GetSearchExhibitorsQuery(string pattern)
		{
			return GetQuery(string.Format("exhibitors?per_page=5&title=$iregex:{0}", pattern)); 
		}

		public string GetUploadUserImageQuery(string wallPostId)
		{
			return GetQuery (string.Format("users/{0}/profile_image", wallPostId));
		}

		public string GetEventVotesByUserQuery(string eventId, string userId)
		{
			return GetQuery (string.Format("user_votes?event={0}&user={1}", eventId, userId));
		}

		public string GetConnectRequestByUserQuery(string userId)
		{
			return GetQuery (string.Format("connections?requester={0}&responder={0}&$mod=$or&order_by=updated_at:desc&order_by=accepted:desc", userId));
		}

		public string GetAcceptedConnectRequestByUserQuery (string userId)
		{
			return GetQuery (string.Format("connections?requester={0}&responder={0}&$mod=$or&order_by=updated_at:desc&accepted=true", userId));
		}

		public string GetConnectRequestByUsersQuery(string requesterId, string responderId)
		{
			return GetQuery (string.Format("connections?requester={0}&responder={1}", requesterId, responderId));
		}

		public string GetConnectionRequestByIdQuery (string id)
		{
			return GetQuery (string.Format ("connections/{0}", id));
		}

		public string GetSearchUsersQuery(string pattern)
		{
			return GetQuery(string.Format("users?name=$iregex:{0}", pattern)); 
		}

		public string GetPostRequestConnectionQuery(string requestId = null)
		{
			if(!string.IsNullOrEmpty(requestId))
				return GetQuery(string.Format("connections/{0}", requestId));
			
			return GetQuery ("connections");
		}

		public string GetRankingQuery()
		{
			return GetQuery("users?per_page=20&order_by=points:desc");
		}

		public string GetResetPasswordQuery()
		{
			return GetQuery ("users/reset-password");
		}

		public string GetPostFavouriteEventQuery()
		{
			return GetQuery ("favorites_actions");
		}

		public string GetDeleteFavEventsQuery (string id)
		{
			return GetQuery (string.Format("favorites_actions/{0}", id));
		}

		public string GetUserFavouriteEventQuery(string userId)
		{
			return GetQuery (string.Format("favorites_actions?user={0}", userId));
		}

		public string GetIsFavouriteEventByUserQuery(string userId, string eventId)
		{
			return GetQuery (string.Format("favorites_actions?user={0}&event={1}", userId, eventId));
		}

		public string GetBadgesActionsByUserQuery(string userId)
		{
			return GetQuery (string.Format("badges_actions?user={0}", userId));
		}

		public string GetPostBadgesActionQuery()
		{
			return GetQuery ("badges_actions");
		}

		public string GetPostUserProfileChangesQuery(string userId)
		{
			return GetQuery (string.Format("users/{0}", userId));
		}
	}
}