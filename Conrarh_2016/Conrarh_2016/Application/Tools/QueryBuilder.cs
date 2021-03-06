﻿using Conarh_2016.Core;
using System;
using System.Linq;
using System.Text;

namespace Conarh_2016.Application.Tools
{
    public sealed class QueryBuilder : Singletone<QueryBuilder>
    {
        internal string ApiUrl = "http://conarh-homol.goldarkapi.com";
        internal string KinveyApiUrl = "https://baas.kinvey.com/appdata/kid_S1uMVFSv";
        internal string KinveyApiUserUrl = "https://baas.kinvey.com/user/kid_S1uMVFSv";
        internal string KinveyApiBlobUrl = "https://baas.kinvey.com/blob/kid_S1uMVFSv";
        internal string KinveyApiResetEmailbUrl = "https://baas.kinvey.com/rpc/kid_S1uMVFSv";

        public static string CombinePath(params string[] segments)
        {
            return segments.Aggregate((total, next) => String.Format("{0}/{1}", total.TrimEnd('/'), next));
        }

        public string GetQuery(params string[] parts)
        {
            return CombinePath(ApiUrl, CombinePath(parts));
        }

        public string GetKinveyQuery(params string[] parts)
        {
            return CombinePath(KinveyApiUrl, CombinePath(parts));
        }

        /*
        public string GetEventsQuery(bool isFree)
        {
            return GetQuery(string.Format("events?free_attending={0}&per_page=8&order_by=date:asc", isFree.ToString().ToLower()));
        }
        */

        public string GetEventsKinveyQuery(bool isFree)
        {
            String _sFree = isFree.ToString().ToLower();
            StringBuilder MyStringBuilder = new StringBuilder("events?query={\"free_attending\":");
            MyStringBuilder.Append(_sFree);
            MyStringBuilder.Append("}&sort={\"date\": 1,\"time_schedule\":1}");

            return GetKinveyQuery(MyStringBuilder.ToString()); ;
        }

        /*
        public string GetExhibitorsQuery()
        {
            return GetQuery("exhibitors?per_page=5");
        }
        */

        public string GetExhibitorsKinveyQuery()
        {
            return GetKinveyQuery("exhibitors");
        }

        /*
        public string GetSponsorTypesQuery()
        {
            return GetQuery("sponsor_types?order_by=type:asc");
        }
        */

        public string GetSponsorTypesKinveyQuery()
        {
            return GetKinveyQuery("sponsorType?&sort={\"type\": 1}");
        }

        /*
        public string GetBadgeTypesQuery()
        {
            return GetQuery("badges_list");
        }
        */

        public string GetBadgeTypesKinveyQuery()
        {
            return GetKinveyQuery("badges");
        }

        /*
        public string GetUsersQuery()
        {
            return GetQuery("users");
        }
        */

        public string GetCurrentUserKinveyQuery()
        {
            StringBuilder sb = new StringBuilder(KinveyApiUserUrl);
            sb.Append("/_me");
            return sb.ToString();
        }

        public string GetUsersKinveyQuery()
        {
            return KinveyApiUserUrl;
        }

        /*
        public string GetUsersSortByNameQuery()
        {
            return GetQuery("users?sort_by=name:asc");
        }
        */

        public string GetUsersSortByNameKinveyQuery()
        {
            StringBuilder sb = new StringBuilder(KinveyApiUserUrl);
            sb.Append("?&sort={\"name\":1}");
            return sb.ToString();
        }

        /*
        public string GetLoginUserSessionQuery()
        {
            return GetQuery("sessions");
        }
        */

        /*public string GetWallPostsQuery()
        {
            return GetQuery("wall");
        }*/

        public string GetWallPostskinveyQuery()
        {
            return GetKinveyQuery("wall");
        }

        /*
        public string GetWallPostByIdQuery(string wallPostId)
        {
            return GetQuery(string.Format("wall/{0}", wallPostId));
        }
        */
        public string GetWallPostByIdKinveyQuery(string wallPostId)
        {
            return GetKinveyQuery(string.Format("wall/{0}", wallPostId));
        }

        /*
        public string GetWallPostsLikesQuery(string wallPostId)
        {
            return GetQuery(string.Format("wall_likes?post={0}", wallPostId));
        }
        */
        public string GetWallPostsLikesKinveyQuery(string wallPostId)
        {
            StringBuilder sb = new StringBuilder("wall-likes?query ={\"post\":\"");
            sb.Append(wallPostId);
            sb.Append("\" }");
            return GetKinveyQuery(sb.ToString());
        }

        /*
        public string GetWallPostsLikesQuery(string wallPostId, string userId)
        {
            return GetQuery(string.Format("wall_likes?post={0}&user={1}", wallPostId, userId));
        }
        */

        public string GetWallPostsLikesKinveyQuery(string wallPostId, string userId)
        {
            StringBuilder sb = new StringBuilder("wall-likes?query={\"post\":\"");
            sb.Append(wallPostId);
            sb.Append("\", \"user\":\"");
            sb.Append(userId);
            sb.Append("\"}");
            return GetKinveyQuery(sb.ToString());
        }

        /*
        public string GetPostUserVotesQuery()
        {
            return GetQuery("user_votes");
        }
        */

        public string GetPostSpeakersKinveyQuery()
        {
            return GetKinveyQuery("speechers");
        }

        public string GetPostUserVotesKinveyQuery()
        {
            return GetKinveyQuery("user-votes");
        }

        /*
        public string GetPostUserVotesQuery(string voteId)
        {
            return GetQuery(string.Format("user_votes/{0}", voteId));
        }
        */

        public string GetPostUserVotesKinveyQuery(string voteId)
        {
            return GetKinveyQuery(string.Format("user-votes/{0}", voteId));
        }

        /*
        public string GetEventVotesByUserQuery(string eventId, string userId)
        {
            return GetQuery(string.Format("user_votes?event={0}&user={1}", eventId, userId));
        }
        */

        public string GetEventVotesByUserKinveyQuery(string eventId, string userId)
        {
            StringBuilder sb = new StringBuilder("user-votes?query={\"event\":\"");
            sb.Append(eventId);
            sb.Append("\", \"user\":\"");
            sb.Append(userId);
            sb.Append("\"}");
            return GetKinveyQuery(sb.ToString());
        }

        /*
        public string GetUserByUsernameQuery(string username)
        {
            return GetQuery(string.Format("users?username={0}", username));
        }
        */

        public string GetUserByEmailKinveyQuery(string _email)
        {
            StringBuilder sb = new StringBuilder(KinveyApiUserUrl);
            sb.Append("/?query={\"username\":\"");
            sb.Append(_email);
            sb.Append("\"}");
            return sb.ToString();
        }

        /*
        public string GetPostQuestionOnEventQuery()
        {
            return GetQuery("questions_events");
        }
        */
        public string GetPostQuestionOnEventKinveyQuery()
        {
            return GetKinveyQuery("questions-events");
        }

        /*
        public string GetPostWallPostLikeQuery()
        {
            return GetQuery("wall_likes");
        }
        */
        public string GetPostWallPostLikeKinveyQuery()
        {
            return GetKinveyQuery("wall-likes");
        }


        public string GetPostPushNotificationsQuery()
        {
            return GetQuery("push/devices");
        }

        public string GetPutPushNotificationsQuery(string id)
        {
            return GetQuery(string.Format("push/devices/{0}", id));
        }

        public string GetPushNotificationsByUserQuery(string userid, string platform)
        {
            return GetQuery(string.Format("push/devices?user_id={0}&platform={1}", userid, platform));
        }

        public string GetPushNotificationsByTokenQuery(string token, string platform)
        {
            return GetQuery(string.Format("push/devices?token={0}&platform={1}", token, platform));
        }

        public string GetDeletePushNotificationsQuery(string id)
        {
            return GetQuery(string.Format("push/devices/{0}", id));
        }
        /* no kynvey é por outro método, usa PostImageKinveyBackgroundTask 
        public string GetPostWallPostImageQuery(string wallPostId)
        {
            return GetQuery(string.Format("wall/{0}/image", wallPostId));
        }
        */
        public string GetSearchExhibitorsQuery(string pattern)
        {
            return GetQuery(string.Format("exhibitors?per_page=5&title=$iregex:{0}", pattern));
        }

        public string GetSearchExhibitorsKinveyQuery(string pattern)
        {
            return GetKinveyQuery(string.Format("exhibitors? query = { \"title\":{ \"$regex\":\" ^{0}\" } }", pattern));
        }

        /*
        public string GetUploadUserImageQuery(string wallPostId)
        {
            return GetQuery(string.Format("users/{0}/profile_image", wallPostId));
        }
        */

        /*
        public string GetConnectRequestByUserQuery(string userId)
        {
            return GetQuery(string.Format("connections?requester={0}&responder={0}&$mod=$or&order_by=updated_at:desc&order_by=accepted:desc", userId));
        }
        */

        public string GetConnectRequestedByUserKinveyQuery(string userId)
        {
            StringBuilder sBuilder = new StringBuilder("connections/?query=%7B%22%24or%22%3A%5B%7B%22responderid%22%3A%22");
            sBuilder.Append(userId);
            sBuilder.Append("%22%7D%2C%20%20%7B%22requesterid%22%3A%22");
            sBuilder.Append(userId);
            sBuilder.Append("%22%7D%5D%7D&sort=%7B%22updated_at%22%3A-1%2C%20%22accepted%22%3A%20-1%7D");

            return GetKinveyQuery(sBuilder.ToString());
        }

        /* não é usado.. não migrei
        public string GetAcceptedConnectRequestByUserQuery(string userId)
        {
            return GetQuery(string.Format("connections?requester={0}&responder={0}&$mod=$or&order_by=updated_at:desc&accepted=true", userId));
        }
        */

        /*
        public string GetConnectRequestByUsersQuery(string requesterId, string responderId)
        {
            return GetQuery(string.Format("connections?requester={0}&responder={1}", requesterId, responderId));
        }
        */

        public string GetConnectRequestByUsersKimveyQuery(string requesterId, string responderId)
        {
            StringBuilder sBuilder = new StringBuilder("connections/?query={\"requesterid\":\"");
            sBuilder.Append(requesterId);
            sBuilder.Append("\", \"responderid\":\"");
            sBuilder.Append(responderId);
            sBuilder.Append("\" }");

            return GetKinveyQuery(sBuilder.ToString());
        }

        /*
        public string GetConnectionRequestByIdQuery(string id)
        {
            return GetQuery(string.Format("connections/{0}", id));
        }
        */

        public string GetConnectionRequestByIdKinveyQuery(string id)
        {
            return GetKinveyQuery(string.Format("connections/{0}", id));
        }

        /*
        public string GetSearchUsersQuery(string pattern)
        {
            return GetQuery(string.Format("users?name=$iregex:{0}", pattern));
        }
        */

        public string GetSearchUsersKinveyQuery(string pattern)
        {
            StringBuilder sb = new StringBuilder(KinveyApiUserUrl);
            sb.Append("/?query={\"name\":{\"$regex\":\"^");
            sb.Append(pattern);
            sb.Append("\"}}");
            return sb.ToString();
        }

        /*
        public string GetPostRequestConnectionQuery(string requestId = null)
        {
            if (!string.IsNullOrEmpty(requestId))
                return GetQuery(string.Format("connections/{0}", requestId));

            return GetQuery("connections");
        }
        */

        public string GetPostRequestConnectionKinveyQuery(string requestId = null)
        {
            if (!string.IsNullOrEmpty(requestId))
                return GetKinveyQuery(string.Format("connections/{0}", requestId));

            return GetKinveyQuery("connections");
        }

        /*
        public string GetRankingQuery()
        {
            return GetQuery("users?per_page=20&order_by=points:desc");
        }
        */

        public string GetRankingKinveyQuery()
        {
            StringBuilder sb = new StringBuilder(KinveyApiUserUrl);
            sb.Append("?&sort={\"points\": -1}&limit=10");
            return sb.ToString();
        }

        /*
        public string GetResetPasswordQuery()
        {
            return GetQuery("users/reset-password");
        }
        */

        public string GetPostResetPasswordKinveyQuery(String email)
        {
            StringBuilder sb = new StringBuilder(KinveyApiResetEmailbUrl);
            sb.Append("/");
            sb.Append(email);
            sb.Append("/user-password-reset-initiate");
            return sb.ToString();
        }

        /*
        public string GetPostFavouriteEventQuery()
        {
            return GetQuery("favorites_actions");
        }
        */

        public string GetPostFavouriteEventKinveyQuery()
        {
            return GetKinveyQuery("favorites-actions");
        }

        /*
            public string GetDeleteFavEventsQuery(string id)
            {
            return GetQuery(string.Format("favorites_actions/{0}", id));
        }

            public string GetUserFavouriteEventQuery(string userId)
        {
            return GetQuery(string.Format("favorites_actions?user={0}", userId));
        }

        public string GetIsFavouriteEventByUserQuery(string userId, string eventId)
            {
       return GetQuery(string.Format("favorites_actions?user={0}&event={1}", userId, eventId));
        }
        */

        public string GetDeleteFavEventsKinveyQuery(string id)
        {
            return GetKinveyQuery(string.Format("favorites-actions/{0}", id));
        }

        public string GetUserFavouriteEventKinveyQuery(string userId)
        {
            StringBuilder sb = new StringBuilder("favorites-actions?query={\"user\":\"");
            sb.Append(userId);
            sb.Append("\"}");

            return GetKinveyQuery(sb.ToString());
        }

        public string GetIsFavouriteEventByUserKinveyQuery(string userId, string eventId)
        {
            StringBuilder sb = new StringBuilder("favorites-actions?query={\"userid\":\"");
            sb.Append(userId);
            sb.Append("\", \"eventid\":\"");
            sb.Append(eventId);
            sb.Append("\"}");
            return GetKinveyQuery(sb.ToString());
        }

        /*
        public string GetBadgesActionsByUserQuery(string userId)
        {
            return GetQuery(string.Format("badges_actions?user={0}", userId));
        }
        */

        public string GetBadgesActionsByUserKinveyQuery(string userId)
        {
            StringBuilder MyStringBuilder = new StringBuilder("badges-actions?query={\"user\":\"");
            MyStringBuilder.Append(userId);
            MyStringBuilder.Append("\"}");

            return GetKinveyQuery(MyStringBuilder.ToString());
        }

        /*
        public string GetPostBadgesActionQuery()
        {
            return GetQuery("badges_actions");
        }
        */

        public string GetPostBadgesActionKinveyQuery()
        {
            return GetKinveyQuery("badges-actions");
        }

        /*
		public string GetPostUserProfileChangesQuery(string userId)
		{
			return GetQuery (string.Format("users/{0}", userId));
		}
        */

        public string GetPostUserProfileChangesKinveyQuery(string userId)
        {
            StringBuilder sb = new StringBuilder(KinveyApiUserUrl);
            sb.Append("/");
            sb.Append(userId);
            return sb.ToString();
        }

        public string GetPostLoginUserKinveyQuery()
        {
            StringBuilder sb = new StringBuilder(KinveyApiUserUrl);
            sb.Append("/login");
            return sb.ToString();
        }
    }
}