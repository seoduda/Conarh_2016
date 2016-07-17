using Conarh_2016.Application.DataAccess;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Wrappers;
using Conarh_2016.Core;
using Newtonsoft.Json;
using PushNotification.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Conarh_2016.Application
{
    public sealed class AppModel : Singletone<AppModel>
    {
        public DynamicListData<ImageData> Images;

        public DynamicListData<ConnectRequest> Requests;
        public DynamicListData<SponsorType> SponsorTypes;
        public DynamicListData<BadgeType> BadgeTypes;
        public DynamicListData<EventData> PayedEvents;
        public DynamicListData<EventData> FreeEvents;
        public DynamicListData<Exhibitor> Exhibitors;
        public DynamicListData<WallPost> WallPosts;
        public DynamicListData<User> Users;

        public DynamicListData<WallPostLike> WallPostLikes;
        public DynamicListData<Speaker> Speakers;

        public WallPostsDataWrapper WallPostsWrapper;
        public ExhibitorsDataWrapper ExhibitorsWrapper;
        public EventsDataWrapper FreeEventsWrapper;
        public EventsDataWrapper PayedEventsWrapper;
        public UserModelsWrapper UsersModelsWrapper;

        public ExhibitorsGridWrapper ExhibitorsGridWrapper;
        public EventsGridWrapper FreeEventsGridWrapper;
        public EventsGridWrapper PayedEventsGridWrapper;

        public ConnectionsDataWrapper CurrentConnectionsWrapper;
        public ContactListWrapper CurrentContactListWrapper;

        public event Action<UserModel, UserModel> UserChanged;

        public AppInformation AppInformation;

        public UserModel CurrentUser
        {
            private set;
            get;
        }

        public void UpdatePushNotificationToken(string token, DeviceType deviceType)
        {
            AppInformation.PushNotificationToken = token;
            DbClient.Instance.SaveData<AppInformation>(AppInformation).ConfigureAwait(false);
        }

        public void LoadAppData()
        {
            //			AppProvider.Log.WriteLine (Conarh_2016.Core.Services.LogChannel.All, "GetPoints " + 20.ToString() + AppResources.GetLevelImageByPoints (20));
            //			AppProvider.Log.WriteLine (Conarh_2016.Core.Services.LogChannel.All, "GetPoints " + 120.ToString() + AppResources.GetLevelImageByPoints (120));
            //			AppProvider.Log.WriteLine (Conarh_2016.Core.Services.LogChannel.All, "GetPoints " + 200.ToString() + AppResources.GetLevelImageByPoints (200));
            //
            //			AppProvider.Log.WriteLine (Conarh_2016.Core.Services.LogChannel.All, "GetPoints " + 349.ToString() + AppResources.GetLevelImageByPoints (349));
            //			AppProvider.Log.WriteLine (Conarh_2016.Core.Services.LogChannel.All, "GetPoints " + 580.ToString() + AppResources.GetLevelImageByPoints (580));
            //			AppProvider.Log.WriteLine (Conarh_2016.Core.Services.LogChannel.All, "GetPoints " + 998.ToString() + AppResources.GetLevelImageByPoints (998));
            //			AppProvider.Log.WriteLine (Conarh_2016.Core.Services.LogChannel.All, "GetPoints " + 1002.ToString() + AppResources.GetLevelImageByPoints (1002));

            AppInformation = DbClient.Instance.GetData<AppInformation>(AppInformation.AppDataId).Result ?? new AppInformation();

            Images = new DynamicListData<ImageData>();
            Images.UpdateData(DbClient.Instance.GetData<ImageData>().Result);

            Speakers = new DynamicListData<Speaker>();
            Speakers.UpdateData(DbClient.Instance.GetData<Speaker>().Result);

            BadgeTypes = new DynamicListData<BadgeType>();
            BadgeTypes.UpdateData(DbClient.Instance.GetData<BadgeType>().Result);

            SponsorTypes = new DynamicListData<SponsorType>();
            SponsorTypes.UpdateData(DbClient.Instance.GetData<SponsorType>().Result);

            /* TODO ARumar o load data */
            SponsorTypes.Items.Clear();

            Users = new DynamicListData<User>();
            Users.UpdateData(DbClient.Instance.GetData<User>().Result);

            Requests = new DynamicListData<ConnectRequest>();
            List<ConnectRequest> requestsData = DbClient.Instance.GetData<ConnectRequest>().Result;
            foreach (ConnectRequest request in requestsData)
            {
                request.Requester = Users.Find(request.RequesterId);
                request.Responder = Users.Find(request.ResponderId);
            }
            Requests.UpdateData(requestsData);


            /* TODO ARumar o load data */
            Exhibitors = new DynamicListData<Exhibitor>();
            List<Exhibitor> exhibitorData = DbClient.Instance.GetData<Exhibitor>().Result;
            /*
            foreach (Exhibitor exhibitor in exhibitorData)
                exhibitor.SponsorType = SponsorTypes.Find(exhibitor.SponsorTypeId);
            */
            exhibitorData.Clear();
            Exhibitors.UpdateData(exhibitorData);

            PayedEvents = new DynamicListData<EventData>();
            FreeEvents = new DynamicListData<EventData>();
            List<EventData> eventsData = DbClient.Instance.GetData<EventData>().Result;
            foreach (EventData evData in eventsData)
            {
                evData.Speechers = new List<Speaker>();
                List<string> speakersIds = JsonConvert.DeserializeObject<List<string>>(evData.SpeechersList);
                foreach (string speakerId in speakersIds)
                    evData.Speechers.Add(Speakers.Find(speakerId));
            }
            PayedEvents.UpdateData(eventsData.FindAll(temp => !temp.FreeAttending));
            FreeEvents.UpdateData(eventsData.FindAll(temp => temp.FreeAttending));

            WallPostLikes = new DynamicListData<WallPostLike>();
            List<WallPostLike> likesData = DbClient.Instance.GetData<WallPostLike>().Result;
            foreach (WallPostLike like in likesData)
                like.User = Users.Find(like.UserId);
            WallPostLikes.UpdateData(likesData);

            WallPosts = new DynamicListData<WallPost>();
            List<WallPost> wallPostData = DbClient.Instance.GetData<WallPost>().Result;
            foreach (WallPost wallPost in wallPostData)
            {
                wallPost.CreatedUser = Users.Find(wallPost.CreatedUserId);

                List<WallPostLike> likes = WallPostLikes.Items.FindAll(temp => temp.Post.Equals(wallPost.Id));
                foreach (WallPostLike likeData in likes)
                    wallPost.LikeList.Add(likeData.User);
            }
            WallPosts.UpdateData(wallPostData);

            ExhibitorsWrapper = new ExhibitorsDataWrapper(SponsorTypes, Exhibitors, false);
            FreeEventsWrapper = new EventsDataWrapper(FreeEvents);
            PayedEventsWrapper = new EventsDataWrapper(PayedEvents);
            WallPostsWrapper = new WallPostsDataWrapper(WallPosts);
            UsersModelsWrapper = new UserModelsWrapper(Users);

            FreeEventsGridWrapper = new EventsGridWrapper(FreeEvents);
            PayedEventsGridWrapper = new EventsGridWrapper(PayedEvents);
        }

        public void LoginUser(User user, string password)
        {
            ChangeUser(user, password);
        }

        public void LogoutUser()
        {
            if (CurrentConnectionsWrapper != null)
                CurrentConnectionsWrapper.ClearData();
            CurrentConnectionsWrapper = null;

            if (CurrentContactListWrapper != null)
                CurrentContactListWrapper.ClearData();
            CurrentContactListWrapper = null;

            ChangeUser(null);
        }

        public void InitUserModel(UserModel newUserModel)
        {
            if (newUserModel == null)
                return;
            try
            {
                newUserModel.UpdatePushData(DbClient.Instance.GetPushData(newUserModel.User.Id, AppInformation.PushNotificationToken).Result);
            }
            catch
            {
            }

            List<ConnectRequest> requests = DbClient.Instance.GetUserConnectRequest(newUserModel.User.Id).Result;
            foreach (ConnectRequest request in requests)
            {
                request.Requester = Users.Find(request.RequesterId);
                request.Responder = Users.Find(request.ResponderId);
            }
            newUserModel.Connections.UpdateData(requests);

            newUserModel.FavouriteActions.UpdateData(DbClient.Instance.GetUserFavouriteActions(newUserModel.User.Id).Result);
            newUserModel.VoteData.UpdateData(DbClient.Instance.GetUserEventVotes(newUserModel.User.Id).Result);

            newUserModel.BadgeActions.UpdateData(DbClient.Instance.GetUserBadgeActions(newUserModel.User.Id).Result);
        }

        private void ApplyUser(UserModel newUserModel)
        {
            CurrentUser = newUserModel;

            if (CurrentUser != null)
            {
                InitUserModel(CurrentUser);
                CurrentConnectionsWrapper = new ConnectionsDataWrapper(CurrentUser, UsersModelsWrapper);
                CurrentContactListWrapper = new ContactListWrapper(CurrentUser, CurrentConnectionsWrapper);
            }
        }

        private void ChangeUser(User user, string password = null)
        {
            UserModel oldUser = CurrentUser;
            UserModel newUserModel = user != null ? UsersModelsWrapper.UsersModels.Find(user.Id) : null;
            if (CurrentUser == null || newUserModel == null)
            {
                ApplyUser(newUserModel);

                if (UserChanged != null)
                    UserChanged(oldUser, CurrentUser);
            }

            AppModel.Instance.AppInformation.CurrentUserId = CurrentUser == null ? string.Empty : CurrentUser.User.Id;
            AppModel.Instance.AppInformation.CurrentUserPassword = password;
            DbClient.Instance.SaveData<AppInformation>(AppModel.Instance.AppInformation).ConfigureAwait(false);
        }

        public Dictionary<string, bool> GetBadgesStates(List<BadgeAction> badgesUserResult)
        {
            var result = new Dictionary<string, bool>();

            foreach (BadgeType badgeType in AppModel.Instance.BadgeTypes.Items)
            {
                bool isEnabled = false;
                List<BadgeAction> actions = badgesUserResult.FindAll(temp => temp.Id.Equals(badgeType.Id));
                isEnabled = AppResources.IsBadgeEnabled(badgeType.Id, actions.Count);
                result.Add(badgeType.Id, isEnabled);
            }

            return result;
        }

        public List<WallPostLike> GetWallPostLikes(WallPost model)
        {
            return WallPostLikes.Items.FindAll(temp => temp.Post.Equals(model.Id));
        }

        public bool GetIsEventFavourite(EventData eventData)
        {
            bool result = false;

            if (CurrentUser != null)
            {
                result = CurrentUser.FavouriteActions.Items.Find(temp => temp.Event.Equals(eventData.Id)) != null;
            }
            return result;
        }

        public List<UserVoteData> GetVoteDataByEvent(EventData eventData)
        {
            List<UserVoteData> votes = new List<UserVoteData>();

            votes.Add(GetVote(eventData, UserVoteType.Conteudo));
            votes.Add(GetVote(eventData, UserVoteType.Aplicabilidade));

            foreach (Speaker user in eventData.Speechers)
                votes.Add(GetVote(eventData, UserVoteType.Speaker, user));

            return votes;
        }

        private UserVoteData GetVote(EventData eventData, UserVoteType voteType, Speaker speaker = null)
        {
            UserVoteData item = null;
            string userId = AppModel.Instance.CurrentUser == null ? string.Empty :
                AppModel.Instance.CurrentUser.User.Id;

            if (AppModel.Instance.CurrentUser != null)
            {
                if (speaker == null)
                    item = AppModel.Instance.CurrentUser.VoteData.Items.Find(temp => temp.Event.Equals(eventData.Id) && temp.User.Equals(userId) && temp.UserVoteType == voteType);
                else
                    item = AppModel.Instance.CurrentUser.VoteData.Items.Find(temp => temp.Event.Equals(eventData.Id) &&
                       temp.User.Equals(userId) && temp.UserVoteType == voteType &&
                       temp.Subject.Equals(speaker.Id));
            }
            if (item == null)
                item = new UserVoteData(voteType, eventData.Id, userId, speaker);

            return item;
        }

        public bool IsEmail(string emailString)
        {
            return Regex.IsMatch(emailString, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }
    }
}