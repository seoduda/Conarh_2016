using Acr.UserDialogs;
using Conarh_2016.Application.BackgroundTasks;
using Conarh_2016.Application.BackgroundTasks.GetData.Kinvey;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Domain.PostData;
using Conarh_2016.Application.Tools;
using Conarh_2016.Application.UI.Events;
using Conarh_2016.Application.UI.Main;
using Conarh_2016.Application.Wrappers;
using Conarh_2016.Core;
using Conarh_2016.Core.Exceptions;
using Conarh_2016.Core.Services;
using Core.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Conarh_2016.Application
{
    public sealed class UserController : Singletone<UserController>, IDisposable
    {
        #region IDisposable implementation

        public void Dispose()
        {
        }

        #endregion IDisposable implementation

        private readonly Dictionary<AppBackgroundWorkerType, BackgroundWorker> _backgroundWorkers;

        public RootPage AppRootPage;

        public UserController()
        {
            _backgroundWorkers = new Dictionary<AppBackgroundWorkerType, BackgroundWorker>();
            SetUpBackgroundWorkers();
        }

        private void SetUpBackgroundWorkers()
        {
            if (_backgroundWorkers.Count > 0)
                return;

            AddBackgroundWorker(AppBackgroundWorkerType.UserDefault);
            AddBackgroundWorker(AppBackgroundWorkerType.UserDownloadConnections);
            AddBackgroundWorker(AppBackgroundWorkerType.UserDownloadBadgesActions);
            AddBackgroundWorker(AppBackgroundWorkerType.UserPostData);
            AddBackgroundWorker(AppBackgroundWorkerType.UserDownloadFavouriteEvents);
        }

        private void AddBackgroundWorker(AppBackgroundWorkerType type)
        {
            var worker = new BackgroundWorker();
            _backgroundWorkers.Add(type, worker);
            worker.Start();
        }

        public void RegisterUser(CreateUserData data)
        {
            UserDialogs.Instance.ShowLoading(AppResources.LoadingCreatingUser);

            var registerTask = new RegisterUserBackgroundTask(data);
            registerTask.ContinueWith((task, result) =>
            {
                UserDialogs.Instance.HideLoading();

                if (task.Exception != null)
                {
                    ServerException exception = (ServerException)task.Exception.InnerException;
                    var serverError = JsonConvert.DeserializeObject<ServerError>(exception.ErrorMessage);

                    AppProvider.PopUpFactory.ShowMessage(serverError.ErrorMessage, AppResources.Warning);
                }
                else
                {
                    Device.BeginInvokeOnMainThread(
                        () => AppController.Instance.AppRootPage.NavigateTo(MainMenuItemData.LoginPage, true, result.Email, result.Password));
                }
            });
            _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(registerTask);
        }

        public void Logout()
        {
            if (AppModel.Instance.CurrentUser.PushNotificationData != null)
            {
                UserDialogs.Instance.ShowLoading(AppResources.LoadingLogoutUser);

                string query = QueryBuilder.Instance.GetDeletePushNotificationsQuery(AppModel.Instance.CurrentUser.PushNotificationData.Id);
                var unregisterPushNotificationTask = new DeleteDataBackgroundTask<PushNotificationData>(null, AppModel.Instance.CurrentUser.PushNotificationData, query);
                unregisterPushNotificationTask.ContinueWith((task, result) =>
                {
                    UserDialogs.Instance.HideLoading();
                    AppModel.Instance.LogoutUser();

                    ClearBackgroundWorkers();
                });
                _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(unregisterPushNotificationTask);
            }
            else
            {
                AppModel.Instance.LogoutUser();

                ClearBackgroundWorkers();
            }
        }

        private void ClearBackgroundWorkers(AppBackgroundWorkerType workerType)
        {
            if (!_backgroundWorkers.ContainsKey(workerType))
                return;

            _backgroundWorkers[workerType].Stop();
            _backgroundWorkers[workerType].Dispose();

            _backgroundWorkers.Remove(workerType);
        }

        private void ClearBackgroundWorkers()
        {
            ClearBackgroundWorkers(AppBackgroundWorkerType.UserDefault);
            ClearBackgroundWorkers(AppBackgroundWorkerType.UserDownloadConnections);
            ClearBackgroundWorkers(AppBackgroundWorkerType.UserDownloadBadgesActions);
            ClearBackgroundWorkers(AppBackgroundWorkerType.UserPostData);
            ClearBackgroundWorkers(AppBackgroundWorkerType.UserDownloadFavouriteEvents);

            SetUpBackgroundWorkers();
        }

        public void ShowServerError()
        {
            AppProvider.PopUpFactory.ShowMessage(AppResources.FailedServer, AppResources.Warning);
        }

        public void LoginUser(LoginUserData data)
        {
            UserDialogs.Instance.ShowLoading(AppResources.LoadingLoginUser);
            var loginTask = new LoginBackgroundTask(data);
            loginTask.ContinueWith((task, result) =>
            {
                if (task.Exception != null)
                {
                    ServerException exception = (ServerException)task.Exception.InnerException;

                    if (exception != null)
                    {
                        UserDialogs.Instance.HideLoading();
                        var serverError = JsonConvert.DeserializeObject<ServerError>(exception.ErrorMessage);
                        AppProvider.PopUpFactory.ShowMessage(serverError.ErrorMessage, AppResources.Warning);
                    }
                }
                else
                {
                    if (result != null)
                    {
                        UserDialogs.Instance.HideLoading();
                        Device.BeginInvokeOnMainThread(() => AppModel.Instance.LoginUser(result, data.Password));

                        /* TODO Ativar Push notification -  UserController - Login
                        var getUserPushNotificationData = new DownloadPushNotificationsByUserBackgroundTask(
                            result.Id, AppModel.Instance.AppInformation.PushNotificationPlatform);

                        getUserPushNotificationData.ContinueWith((getPushTask, getPushResult) =>
                        {
                            if (getPushResult == null)
                            {
                                UserDialogs.Instance.HideLoading();
                                ShowServerError();
                            }
                            else
                            {
                                bool isUserHavePushForDevice = getPushResult.Find(temp => temp.UserId.Equals(result.Id) &&
                                    temp.Token.Equals(AppModel.Instance.AppInformation.PushNotificationToken)) != null &&
                                    getPushResult.Count > 0;

                                if (string.IsNullOrEmpty(AppModel.Instance.AppInformation.PushNotificationToken))
                                {
                                    UserDialogs.Instance.HideLoading();
                                    Device.BeginInvokeOnMainThread(() => AppModel.Instance.LoginUser(result, data.Password));
                                }
                                else if (isUserHavePushForDevice)
                                {
                                    UserDialogs.Instance.HideLoading();
                                    Device.BeginInvokeOnMainThread(() => AppModel.Instance.LoginUser(result, data.Password));
                                }
                                else
                                {
                                    //check if current token is registered

                                    string query = QueryBuilder.Instance.GetPushNotificationsByTokenQuery(
                                        AppModel.Instance.AppInformation.PushNotificationToken,
                                        AppModel.Instance.AppInformation.PushNotificationPlatform);

                                    AppProvider.Log.WriteLine(LogChannel.All, query);
                                    var getTokenPushNotificationData = new DownloadPushNotificationsByUserBackgroundTask(query);

                                    getTokenPushNotificationData.ContinueWith((tokenTask, tokenResult) =>
                                    {
                                        if (tokenResult == null)
                                        {
                                            UserDialogs.Instance.HideLoading();
                                            ShowServerError();
                                        }
                                        else
                                        {
                                            bool isPost = tokenResult.Count == 0;
                                            string notifQuery = !isPost ? QueryBuilder.Instance.GetPutPushNotificationsQuery(tokenResult[0].Id) :
                                                QueryBuilder.Instance.GetPostPushNotificationsQuery();

                                            PushNotificationData pushData = new PushNotificationData(AppModel.Instance.AppInformation, result.Id);

                                            var pushNotificationTask = new PostPushNotificationsBackgroundTask(pushData, isPost, notifQuery);
                                            pushNotificationTask.ContinueWith((pushTask, pushTaskResult) =>
                                            {
                                                UserDialogs.Instance.HideLoading();
                                                Device.BeginInvokeOnMainThread(() => AppModel.Instance.LoginUser(result, data.Password));
                                            });
                                            _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(pushNotificationTask);
                                        }
                                    });
                                    _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(getTokenPushNotificationData);
                                }
                            }
                        });
                        _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(getUserPushNotificationData);
                        */
                    }
                }
            });
            _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(loginTask);
        }

        public void PostQuestion(EventData data, string question, Action onDone)
        {
            if (AppModel.Instance.CurrentUser == null)
            {
                AppProvider.PopUpFactory.ShowMessage(AppResources.LoginFirstMessage, AppResources.Warning);
                return;
            }

            UserDialogs.Instance.ShowLoading(AppResources.LoadingSendingQuestion);
            var questionData = new QuestionData
            {
                UserId = AppModel.Instance.CurrentUser.User.Id,
                EventId = data.Id,
                Question = question
            };
            var postQuestionTask = new PostDataBackgroundTask<QuestionData>(QueryBuilder.Instance.GetPostQuestionOnEventQuery(), questionData, true);
            postQuestionTask.ContinueWith((task, result) =>
            {
                UserDialogs.Instance.HideLoading();

                if (result == null)
                    AppProvider.PopUpFactory.ShowMessage(AppResources.FailedServer, AppResources.Error);
                else
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (onDone != null)
                            onDone.Invoke();
                        UserDialogs.Instance.ShowSuccess(AppResources.SuccessfulPostMessage, 2);
                    });
            });
            _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(postQuestionTask);
        }

        public void PostVote(UserVoteData voteData, bool newState, LikedItem item)
        {
            if (AppModel.Instance.CurrentUser == null)
            {
                AppProvider.PopUpFactory.ShowMessage(AppResources.LoginFirstMessage, AppResources.Warning);
                return;
            }

            UserDialogs.Instance.ShowLoading(AppResources.LoadingSendingVote);
            string previousVote = voteData.Vote;
            voteData.SetState(newState);

            var postVoteTask = new PostEventVoteBackgroundTask(voteData, AppModel.Instance.CurrentUser.VoteData);
            postVoteTask.ContinueWith((task, result) =>
            {
                UserDialogs.Instance.HideLoading();

                if (result == null)
                {
                    AppProvider.PopUpFactory.ShowMessage(AppResources.FailedServer, AppResources.Error);
                    voteData.Vote = previousVote;
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        UserDialogs.Instance.ShowSuccess(AppResources.SuccessfulPostVote, 1);
                        item.Update();
                    });
                }
            });
            _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(postVoteTask);
        }

        public void LikeWallPost(WallPost wallPost)
        {
            if (AppModel.Instance.CurrentUser == null)
            {
                AppProvider.PopUpFactory.ShowMessage(AppResources.LoginFirstMessage, AppResources.Warning);
                return;
            }

            if (wallPost.LikeList.Find(temp => temp.Id.Equals(AppModel.Instance.CurrentUser.User.Id)) != null)
            {
                AppProvider.PopUpFactory.ShowMessage(AppResources.AlreadyLikePostMessage, string.Empty);
                return;
            }

            AppProvider.PopUpFactory.ShowConfirmation(AppResources.LikePostMessageQuestion, string.Empty, () => LikeWallPostByUser(wallPost), null, AppResources.YesButtonName, AppResources.NoButtonName);
        }

        private void LikeWallPostByUser(WallPost post)
        {
            UserDialogs.Instance.ShowLoading(AppResources.LoadingSendingLike);

            string userId = AppModel.Instance.CurrentUser.User.Id;
            var getIsLikedPostTask = new DownloadWallPostLikesBackgroundTask(post, userId);

            getIsLikedPostTask.ContinueWith((resultIsLikedTask, isLikedResult) =>
            {
                if (isLikedResult.Count > 0)
                {
                    UserDialogs.Instance.HideLoading();
                    AppProvider.PopUpFactory.ShowMessage(AppResources.AlreadyLikePostMessage, string.Empty);
                }
                else
                {
                    var postLikeTask = new PostWallLikeBackgroundTask(AppModel.Instance.CurrentUser.User, post);
                    postLikeTask.ContinueWith((task, result) =>
                    {
                        if (result == null)
                        {
                            UserDialogs.Instance.HideLoading();
                            AppProvider.PopUpFactory.ShowMessage(AppResources.FailedServer, AppResources.Error);
                        }
                        else
                        {
                            string query = QueryBuilder.Instance.GetWallPostByIdQuery(post.Id);
                            var getNewWallPostData = new GetItemByIdBackgroundTask<WallPost>(query, AppModel.Instance.WallPosts);
                            getNewWallPostData.ContinueWith((getTask, getResult) =>
                            {
                                UserDialogs.Instance.HideLoading();
                                if (getResult == null)
                                    AppProvider.PopUpFactory.ShowMessage(AppResources.FailedServer, AppResources.Error);
                                else
                                    UserDialogs.Instance.ShowSuccess(AppResources.SuccessfulPostVote, 1);
                            });
                            _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(getNewWallPostData);
                        }
                    });
                    _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(postLikeTask);
                }
            });
            _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(getIsLikedPostTask);
        }

        public void CreateWallPost(string text, string imagePath)
        {
            UserDialogs.Instance.ShowLoading(AppResources.LoadingSendingWallPost);
            var data = new CreateWallPostData { UserId = AppModel.Instance.CurrentUser.User.Id, Text = text };
            string query = QueryBuilder.Instance.GetWallPostsQuery();

            var createWallPostTask = new PostDataBackgroundTask<CreateWallPostData>(query, data);
            createWallPostTask.ContinueWith((task, result) =>
            {
                if (result == null)
                {
                    UserDialogs.Instance.HideLoading();
                    AppProvider.PopUpFactory.ShowMessage(AppResources.FailedServer, AppResources.Error);
                }
                else
                {
                    if (string.IsNullOrEmpty(imagePath))
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            UserDialogs.Instance.ShowSuccess(AppResources.SuccessfulCreateWallPost, 1);
                            AppController.Instance.AppRootPage.Detail.Navigation.PopAsync();

                            AppController.Instance.DownloadWallData(null);
                        });

                        _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(
                            new PostBadgeActionBackgroundTask(AppBadgeType.WallPost, AppModel.Instance.CurrentUser.User.Id, AppModel.Instance.CurrentUser.BadgeActions));
                    }
                    else
                    {
                        var postImageToPost = new PostImageBackgroundTask(QueryBuilder.Instance.GetPostWallPostImageQuery(result.Id), imagePath);
                        postImageToPost.ContinueWith((pItask, pIresult) =>
                        {
                            if (pIresult == null)
                            {
                                UserDialogs.Instance.HideLoading();
                                AppProvider.PopUpFactory.ShowMessage(AppResources.FailedServer, AppResources.Error);
                            }
                            else
                            {
                                _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(
                                    new PostBadgeActionBackgroundTask(AppBadgeType.WallPost, AppModel.Instance.CurrentUser.User.Id, AppModel.Instance.CurrentUser.BadgeActions));

                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    UserDialogs.Instance.ShowSuccess(AppResources.SuccessfulCreateWallPost, 1);
                                    AppController.Instance.AppRootPage.Detail.Navigation.PopAsync();

                                    AppController.Instance.DownloadWallData(null);
                                });
                            }
                        });
                        _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(postImageToPost);
                    }
                }
            });
            _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(createWallPostTask);
        }

        public void UpdateEventUserVotes(UserEventActionsModel eventActionsModel)
        {
            if (AppModel.Instance.CurrentUser == null)
                return;

            UserDialogs.Instance.ShowLoading(AppResources.LoadingUpdateEventVotes);
            var downloadVotesTask = new DownloadEventUserVotesBackgroundTask(eventActionsModel.Data.Id, AppModel.Instance.CurrentUser.User.Id);
            downloadVotesTask.ContinueWith((task, result) =>
            {
                UserDialogs.Instance.HideLoading();

                if (result != null)
                    Device.BeginInvokeOnMainThread(() => eventActionsModel.Update(result));
            });
            _backgroundWorkers[AppBackgroundWorkerType.UserDefault].Add(downloadVotesTask);
        }

        public void AddEventToFavourites(EventData eventData)
        {
            if (AppModel.Instance.CurrentUser == null)
            {
                AppProvider.PopUpFactory.ShowMessage(AppResources.LoginFirstMessage, AppResources.Warning);
                return;
            }

            UserDialogs.Instance.ShowLoading(AppResources.LoadingFavouriteActions);
            FavouriteEventData favData = AppModel.Instance.CurrentUser.FavouriteActions.Items.Find(temp => temp.Event.Equals(eventData.Id));

            if (favData != null)
            {
                string query = QueryBuilder.Instance.GetDeleteFavEventsQuery(favData.Id);
                var deleteFavEventTask = new DeleteDataBackgroundTask<FavouriteEventData>(AppModel.Instance.CurrentUser.FavouriteActions, favData, query);
                deleteFavEventTask.ContinueWith((task, result) => UserDialogs.Instance.HideLoading());
                _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(deleteFavEventTask);
            }
            else
            {
                var getIsFavouriteEventTask = new DownloadFavouriteEventsBackgroundTask(eventData.Id, AppModel.Instance.CurrentUser.User.Id,
                    AppModel.Instance.CurrentUser.FavouriteActions);

                getIsFavouriteEventTask.ContinueWith((resultIsFavTask, isFavResult) =>
                {
                    if (isFavResult.Count > 0)
                    {
                        UserDialogs.Instance.HideLoading();
                        AppProvider.PopUpFactory.ShowMessage(AppResources.AlreadyLikePostMessage, string.Empty);

                        string query = QueryBuilder.Instance.GetDeleteFavEventsQuery(favData.Id);
                        var deleteFavEventTask = new DeleteDataBackgroundTask<FavouriteEventData>(AppModel.Instance.CurrentUser.FavouriteActions, favData, query);
                        deleteFavEventTask.ContinueWith((task, result) => UserDialogs.Instance.HideLoading());
                        _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(deleteFavEventTask);
                    }
                    else
                    {
                        var addToFavouriteTask = new PostFavouriteEventBackgroundTask(AppModel.Instance.CurrentUser.User, eventData,
                            AppModel.Instance.CurrentUser.FavouriteActions);

                        addToFavouriteTask.ContinueWith((task, result) =>
                        {
                            UserDialogs.Instance.HideLoading();
                            if (result == null)
                                AppProvider.PopUpFactory.ShowMessage(AppResources.FailedServer, AppResources.Error);
                            else
                                UserDialogs.Instance.ShowSuccess(AppResources.SuccessfullFavAdd, 1);
                        });
                        _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(addToFavouriteTask);
                    }
                });
                _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(getIsFavouriteEventTask);
            }
        }

        public void GoToBackground()
        {
            foreach (BackgroundWorker worker in _backgroundWorkers.Values)
                worker.Stop();
        }

        public void ReturnFromBackground()
        {
            foreach (BackgroundWorker worker in _backgroundWorkers.Values)
                worker.Start();
        }

        public void DownloadConnections(Action onFinish = null, bool lookAccepted = false)
        {
            string userId = AppModel.Instance.CurrentUser.User.Id;
            string query = QueryBuilder.Instance.GetConnectRequestByUserQuery(userId);

            AppProvider.Log.WriteLine(LogChannel.All, query);
            var task = new DownloadConnectRequestDataByUserBackgroundTask(AppModel.Instance.CurrentUser.Connections, query);
            task.ContinueWith((rtask, result) => Device.BeginInvokeOnMainThread(onFinish));
            _backgroundWorkers[AppBackgroundWorkerType.UserDownloadConnections].Add(task);
        }

        public void DownloadFavouriteEvents()
        {
            var task = new DownloadFavouriteEventsBackgroundTask(AppModel.Instance.CurrentUser.User,
                AppModel.Instance.CurrentUser.FavouriteActions);

            _backgroundWorkers[AppBackgroundWorkerType.UserDownloadFavouriteEvents].Add(task);
        }

        private void SentCommonRequest(ConnectionModel connectionModel)
        {
            UserDialogs.Instance.ShowLoading(AppResources.LoadingSendingRequest);

            string currentUserId = AppModel.Instance.CurrentUser.User.Id;
            string requestedUserId = connectionModel.UserModel.User.Id;

            var checkIsCurrentUserSendRequest = new DownloadConnectRequestDataByUserBackgroundTask(AppModel.Instance.CurrentUser.Connections,
                QueryBuilder.Instance.GetConnectRequestByUsersQuery(currentUserId, requestedUserId));

            checkIsCurrentUserSendRequest.ContinueWith((task, result) =>
            {
                if (result == null || result.Count > 0)
                {
                    Device.BeginInvokeOnMainThread(() => connectionModel.ApplyConnectRequest(result[0]));
                    UserDialogs.Instance.HideLoading();
                    AppProvider.PopUpFactory.ShowMessage(AppResources.AlreadySentRequestError, AppResources.Error);
                }
                else
                {
                    if (result.Count == 0)
                    {
                        var checkIsUserSendRequestToCurrentUser = new DownloadConnectRequestDataByUserBackgroundTask(AppModel.Instance.CurrentUser.Connections,
                            QueryBuilder.Instance.GetConnectRequestByUsersQuery(requestedUserId, currentUserId));

                        checkIsUserSendRequestToCurrentUser.ContinueWith((rtask, rresult) =>
                        {
                            if (rtask == null || rresult.Count > 0)
                            {
                                Device.BeginInvokeOnMainThread(() => connectionModel.ApplyConnectRequest(rresult[0]));
                                UserDialogs.Instance.HideLoading();
                                AppProvider.PopUpFactory.ShowMessage(AppResources.AlreadySentRequestToError, AppResources.Error);
                            }
                            else if (rresult.Count == 0)
                            {
                                var data = new RequestConnectionData(AppModel.Instance.CurrentUser.User.Id, connectionModel.UserModel.User.Id);

                                var requestTask = new RequestConnectionBackgroundTask(data, false, null);
                                requestTask.ContinueWith((retask, reresult) =>
                                {
                                    if (reresult == null)
                                    {
                                        UserDialogs.Instance.HideLoading();
                                        AppProvider.PopUpFactory.ShowMessage(AppResources.FailedServer, AppResources.Error);
                                    }
                                    else
                                    {
                                        string query = QueryBuilder.Instance.GetConnectionRequestByIdQuery(reresult.Id);
                                        var getRequestTask = new GetItemByIdBackgroundTask<ConnectRequest>(query, connectionModel.UserModel.Connections);

                                        getRequestTask.ContinueWith((getTask, getResult) =>
                                        {
                                            UserDialogs.Instance.HideLoading();
                                            if (getResult != null)
                                            {
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    UserDialogs.Instance.ShowSuccess(AppResources.SuccessfulRequestConnection, 1);
                                                    connectionModel.ApplyConnectRequest(getResult);
                                                });
                                            }
                                        });

                                        _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(getRequestTask);
                                    }
                                });
                                _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(requestTask);
                            }
                        });

                        _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(checkIsUserSendRequestToCurrentUser);
                    }
                }
            });

            _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(checkIsCurrentUserSendRequest);
        }

        private void AcceptCommonRequest(ConnectionModel connectionModel)
        {
            UserDialogs.Instance.ShowLoading(AppResources.LoadingSendingAcceptRequest);

            var data = new RequestConnectionData(connectionModel.Request.Requester.Id,
                connectionModel.Request.Responder.Id,
                AppResources.GetPointsEarned(connectionModel.Request.Responder.UserType),
                true);

            var requestTask = new RequestConnectionBackgroundTask(data, true, connectionModel.Request.Id);
            requestTask.ContinueWith((task, result) =>
            {
                if (result == null)
                {
                    UserDialogs.Instance.HideLoading();
                    AppProvider.PopUpFactory.ShowMessage(AppResources.FailedServer, AppResources.Error);
                }
                else
                {
                    var postBadgeTask = new PostBadgeActionBackgroundTask(AppBadgeType.ConnectTo50Users, AppModel.Instance.CurrentUser.User.Id, AppModel.Instance.CurrentUser.BadgeActions);
                    postBadgeTask.ContinueWith((badgeTask, badgeResult) =>
                    {
                        string query = QueryBuilder.Instance.GetConnectionRequestByIdQuery(connectionModel.Request.Id);
                        var getRequestTask = new GetItemByIdBackgroundTask<ConnectRequest>(query, connectionModel.UserModel.Connections);

                        getRequestTask.ContinueWith((getTask, getResult) =>
                        {
                            UserDialogs.Instance.HideLoading();
                            if (getResult != null)
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    if (Device.OS == TargetPlatform.iOS)
                                        UserDialogs.Instance.ShowSuccess(AppResources.SuccessfulAcceptRequestConnection, 1);

                                    connectionModel.ApplyConnectRequest(getResult);
                                    UpdateProfileData(AppModel.Instance.CurrentUser);
                                });
                            }
                        });

                        _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(getRequestTask);
                    });

                    _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(postBadgeTask);
                    _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(new PostBadgeActionBackgroundTask(AppBadgeType.ConnectTo50Users, connectionModel.UserModel.User.Id, null));
                }
            });
            _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(requestTask);
        }

        private void ConnectToUncommonUser(ConnectionModel connectionModel)
        {
            UserDialogs.Instance.PromptAsync(new PromptConfig
            {
                Title = AppResources.RequestEnterPassphrase,
                Placeholder = AppResources.RequestEnterPassphraseDefault,
                IsCancellable = true
            }).ContinueWith(prompttask =>
            {
                PromptResult promptResult = prompttask.Result;

                if (promptResult.Ok)
                {
                    if (connectionModel.UserModel.User.Passphrase.Equals(promptResult.Text))
                    {
                        UserDialogs.Instance.ShowLoading(AppResources.LoadingSendingRequest);
                        var data = new RequestConnectionData(AppModel.Instance.CurrentUser.User.Id, connectionModel.UserModel.User.Id,
                            AppResources.GetPointsEarned(connectionModel.UserModel.User.UserType), true);

                        var requestTask = new RequestConnectionBackgroundTask(data, true, null);
                        requestTask.ContinueWith((task, result) =>
                        {
                            if (result == null)
                            {
                                UserDialogs.Instance.HideLoading();
                                AppProvider.PopUpFactory.ShowMessage(AppResources.FailedServer, AppResources.Error);
                            }
                            else
                            {
                                var postBadgeTask = new PostBadgeActionBackgroundTask(AppBadgeType.ConnectToUncommonUser, AppModel.Instance.CurrentUser.User.Id, AppModel.Instance.CurrentUser.BadgeActions);
                                postBadgeTask.ContinueWith((badgeTask, badgeResult) =>
                                {
                                    string query = QueryBuilder.Instance.GetConnectionRequestByIdQuery(result.Id);
                                    var getRequestTask = new GetItemByIdBackgroundTask<ConnectRequest>(query, connectionModel.UserModel.Connections);

                                    getRequestTask.ContinueWith((getTask, getResult) =>
                                    {
                                        UserDialogs.Instance.HideLoading();
                                        if (getResult != null)
                                        {
                                            Device.BeginInvokeOnMainThread(() =>
                                            {
                                                UserDialogs.Instance.ShowSuccess(AppResources.SuccessfulAcceptRequestConnection, 1);
                                                connectionModel.ApplyConnectRequest(getResult);
                                                UpdateProfileData(AppModel.Instance.CurrentUser);
                                            });
                                        }
                                    });

                                    _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(getRequestTask);
                                });
                                _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(postBadgeTask);
                            }
                        });
                        _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(requestTask);
                    }
                    else
                        AppProvider.PopUpFactory.ShowMessage(AppResources.RequestEnterPassphraseWrong, AppResources.Error);
                }
            });
        }

        public void TryToConnect(ConnectionModel connectionModel)
        {
            if (AppResources.IsUserCommon(connectionModel.UserModel.User.UserType))
            {
                if (connectionModel.State == ConnectState.RequestNotSent)
                    SentCommonRequest(connectionModel);
                else if (connectionModel.State == ConnectState.RequestedToAccept)
                    AcceptCommonRequest(connectionModel);
            }
            else
                ConnectToUncommonUser(connectionModel);
        }

        public void SaveProfileChanges(CreateUserData profileChanges)
        {
            Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.ShowLoading(AppResources.LoadingSavingProfileChanges));

            var saveProfileChanges = new RegisterUserBackgroundTask(profileChanges, false, AppModel.Instance.CurrentUser.User.Id);
            saveProfileChanges.ContinueWith((task, result) =>
            {
                UserDialogs.Instance.HideLoading();

                if (task.Exception != null)
                {
                    ServerException exception = (ServerException)task.Exception.InnerException;
                    var serverError = JsonConvert.DeserializeObject<ServerError>(exception.ErrorMessage);

                    AppProvider.PopUpFactory.ShowMessage(serverError.ErrorMessage, AppResources.Warning);
                }
                else
                {
                    Device.BeginInvokeOnMainThread(
                        () => AppController.Instance.AppRootPage.NavigateTo(MainMenuItemData.ProfilePage, true));
                }
            });
            _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(saveProfileChanges);
        }

        private void OnShareExecuted()
        {
            if (AppModel.Instance.CurrentUser != null)
            {
                var postBadgeTask = new PostBadgeActionBackgroundTask(AppBadgeType.ShareApp, AppModel.Instance.CurrentUser.User.Id, AppModel.Instance.CurrentUser.BadgeActions);

                postBadgeTask.ContinueWith((badgeTask, badgeResult) =>
                {
                    UpdateProfileData(AppModel.Instance.CurrentUser);
                });
                _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(postBadgeTask);
            }
        }

        public void Share()
        {
            AppProvider.ShareService.ShareLink(AppResources.ShareTitle, AppResources.ShareMessage, AppResources.ShareLink, OnShareExecuted);
        }

        public void UpdateProfileData(UserModel userModel, bool downloadConnections = true, Action onFinish = null)
        {
            UserDialogs.Instance.ShowLoading(AppResources.LoadingHistory);

            //download user profile

            var downloadProfileTask = new DownloadUsersBackgroundTask(null, new KinveyDownloadListParameters(KinveyDownloadCountType.FirstPage,
                QueryBuilder.Instance.GetUserByEmailKinveyQuery(userModel.User.Email)));
            downloadProfileTask.ContinueWith((profileTask, profileResult) =>
            {
                if (profileResult == null)
                {
                    UserDialogs.Instance.HideLoading();
                    AppProvider.PopUpFactory.ShowMessage(AppResources.FailedServer, AppResources.Error);
                }
                else
                {
                    userModel.UpdateUser(profileResult[0]); //points, score, etc
                    UserDialogs.Instance.HideLoading();
                    /*
                    var downloadBadges = new DownloadBadgesTypesBackgroundTask();
                    downloadBadges.ContinueWith((badgesTask, badgesResult) =>
                    {
                        if (badgesResult == null)
                        {
                            UserDialogs.Instance.HideLoading();
                            AppProvider.PopUpFactory.ShowMessage(AppResources.FailedServer, AppResources.Error);
                        }
                        else
                        {
                            var downloadBadgesByUserTask = new DownloadBadgesByUserBackgroundTask(userModel.BadgeActions, userModel.User.Id);
                            downloadBadgesByUserTask.ContinueWith((badgesUserTask, badgesUserResult) =>
                            {
                                UserDialogs.Instance.HideLoading();

                                if (badgesUserResult == null)
                                    AppProvider.PopUpFactory.ShowMessage(AppResources.FailedServer, AppResources.Error);
                                else if (downloadConnections)
                                    DownloadConnections(onFinish: onFinish);
                                else
                                    Device.BeginInvokeOnMainThread(onFinish);
                            });
                            _backgroundWorkers[AppBackgroundWorkerType.UserDefault].Add(downloadBadgesByUserTask);
                        }
                    });
                    _backgroundWorkers[AppBackgroundWorkerType.UserDefault].Add(downloadBadges);
                    */
                }
            });

            _backgroundWorkers[AppBackgroundWorkerType.UserDefault].Add(downloadProfileTask);
        }
        public void RegisterUserLinkedin(CreateUserData data)
        {
            UserDialogs.Instance.ShowLoading(AppResources.LoadingCreatingUser);

            var registerTask = new RegisterUserBackgroundTask(data);
            registerTask.ContinueWith((task, result) =>
            {
                UserDialogs.Instance.HideLoading();

                if (task.Exception != null)
                {
                    ServerException exception = (ServerException)task.Exception.InnerException;
                    var serverError = JsonConvert.DeserializeObject<ServerError>(exception.ErrorMessage);

                    AppProvider.PopUpFactory.ShowMessage(serverError.ErrorMessage, AppResources.Warning);
                }
                else
                {
                    LoginUserData lud = new LoginUserData();
                    lud.Email = result.Email;
                    lud.Password = result.Password;
                    LoginUser(lud);

                    //Device.BeginInvokeOnMainThread(() => AppController.Instance.AppRootPage.NavigateTo(MainMenuItemData.ProfilePage, true));
                    // () => AppController.Instance.AppRootPage.NavigateTo(MainMenuItemData.LoginPage, true, result.Email, result.Password));
                }
            });
            _backgroundWorkers[AppBackgroundWorkerType.UserPostData].Add(registerTask);
        }
    }
}