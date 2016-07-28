using Acr.UserDialogs;
using Conarh_2016.Application.BackgroundTasks;
using Conarh_2016.Application.BackgroundTasks.GetData;
using Conarh_2016.Application.BackgroundTasks.GetData.Kinvey;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Tools;
using Conarh_2016.Application.UI.Main;
using Conarh_2016.Core;
using Conarh_2016.Core.Exceptions;
using Core.Tasks;
using Newtonsoft.Json;
using PushNotification.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Platform.Services.Media;

namespace Conarh_2016.Application
{
    public enum AppBackgroundWorkerType
    {
        DownloadWallPosts = 0,
        DownloadFreeEvents = 1,
        DownloadPayedEvents = 2,
        DownloadExhibitors = 3,
        DownloadSponsorTypes = 4,
        DownloadBadgesTypes = 5,
        SearchExhibitors = 6,
        DownloadUsers = 7,
        SearchUsers = 8,
        DownloadRanking = 9,
        DownloadWallPostLikes = 10,
        DownloadSpeakers = 11,

        DefaultApp = 20,

        UserPostData = 21,
        UserDownloadConnections = 22,
        UserDownloadBadgesActions = 23,
        UserDefault = 24,
        UserDownloadFavouriteEvents = 25
    }

    public sealed class AppController : Singletone<AppController>, IDisposable
    {
        private readonly Dictionary<AppBackgroundWorkerType, BackgroundWorker> _backgroundWorkers;

        public RootPage AppRootPage;

        public AppController()
        {
            _backgroundWorkers = new Dictionary<AppBackgroundWorkerType, BackgroundWorker>();

            AddBackgroundWorker(AppBackgroundWorkerType.DownloadExhibitors);
            AddBackgroundWorker(AppBackgroundWorkerType.DownloadFreeEvents);
            AddBackgroundWorker(AppBackgroundWorkerType.DownloadPayedEvents);
            AddBackgroundWorker(AppBackgroundWorkerType.DownloadWallPosts);
            AddBackgroundWorker(AppBackgroundWorkerType.DownloadSponsorTypes);
            AddBackgroundWorker(AppBackgroundWorkerType.DownloadUsers);
            AddBackgroundWorker(AppBackgroundWorkerType.DownloadRanking);
            AddBackgroundWorker(AppBackgroundWorkerType.DefaultApp);
            AddBackgroundWorker(AppBackgroundWorkerType.DownloadWallPostLikes);

            AddBackgroundWorker(AppBackgroundWorkerType.SearchUsers);
            AddBackgroundWorker(AppBackgroundWorkerType.SearchExhibitors);
        }

        public void Start()
        {
            AppModel.Instance.LoadAppData();
            Device.BeginInvokeOnMainThread(() =>
            {
                if (AppModel.Instance.CurrentUser != null)
                    AppRootPage.NavigateTo(MainMenuItemData.ProfilePage, false);
                else if (!string.IsNullOrEmpty(AppModel.Instance.AppInformation.CurrentUserId))
                {
                    User user = AppModel.Instance.Users.Find(AppModel.Instance.AppInformation.CurrentUserId);
                    AppRootPage.NavigateTo(MainMenuItemData.LoginPage, true, user.Email, AppModel.Instance.AppInformation.CurrentUserPassword);
                }
                else
                    AppRootPage.NavigateTo(MainMenuItemData.LoginPage, false);
            });
        }

        private void AddBackgroundWorker(AppBackgroundWorkerType type)
        {
            var worker = new BackgroundWorker();
            _backgroundWorkers.Add(type, worker);
            worker.Start();
        }

        public void DownloadEventsData(bool isFree, Action onFinish)
        {
            DynamicListData<EventData> events = isFree ? AppModel.Instance.FreeEvents :
                AppModel.Instance.PayedEvents;

            var task = new DownloadEventsDataBackgroundTask(isFree, events);
            task.ContinueWith((ttask, tresult) => Device.BeginInvokeOnMainThread(onFinish));
            if (isFree)
                _backgroundWorkers[AppBackgroundWorkerType.DownloadFreeEvents].Add(task);
            else
                _backgroundWorkers[AppBackgroundWorkerType.DownloadPayedEvents].Add(task);
        }

        public void DownloadExhibitorsData(Action onFinish)
        {
            var downloadSponsorsTask = new DownloadSponsorTypesBackgroundTask();
            downloadSponsorsTask.ContinueWith((task, result) =>
                {
                    if (result != null)
                    {
                        /*
                        var parameters = new DownloadListParameters(DownloadCountType.All,
                            QueryBuilder.Instance.GetExhibitorsQuery());
                         */
                        var parameters = new KinveyDownloadListParameters(KinveyDownloadCountType.All,
                           QueryBuilder.Instance.GetExhibitorsKinveyQuery());

                        var downloadTask = new DownloadExhibitorsBackgroundTask(AppModel.Instance.Exhibitors, parameters);
                        downloadTask.ContinueWith((ttask, tresult) => Device.BeginInvokeOnMainThread(onFinish));
                        _backgroundWorkers[AppBackgroundWorkerType.DownloadExhibitors].Add(downloadTask);
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(onFinish);
                        AppProvider.PopUpFactory.ShowMessage(AppResources.FailedServer, AppResources.Warning);
                    }
                });
            _backgroundWorkers[AppBackgroundWorkerType.DownloadSponsorTypes].Add(downloadSponsorsTask);
        }


        public void DownloadSpeakersData(Action onFinish)
        {
            var downloadSpeakersTask = new DownloadSpeakersBackgroundTask();
            downloadSpeakersTask.ContinueWith((task, result) => Device.BeginInvokeOnMainThread(onFinish));
            _backgroundWorkers[AppBackgroundWorkerType.DownloadSpeakers].Add(downloadSpeakersTask);
        }


        public void DownloadWallData(Action doneAction)
        {
            var downloadPosts = new DownloadWallPostsBackgroundTask(AppModel.Instance.WallPosts);
            downloadPosts.ContinueWith((task, result) => Device.BeginInvokeOnMainThread(doneAction));
            _backgroundWorkers[AppBackgroundWorkerType.DownloadWallPosts].Add(downloadPosts);
        }

        public void SearchExhibitors(string pattern, DynamicListData<Exhibitor> searchModel, Action onFinish)
        {
            UserDialogs.Instance.ShowLoading(AppResources.LoadingSearchExhibitors);
            var parameters = new KinveyDownloadListParameters(KinveyDownloadCountType.All,
                                 QueryBuilder.Instance.GetSearchExhibitorsKinveyQuery(pattern));

            var searchTask = new DownloadExhibitorsBackgroundTask(searchModel, parameters);
            searchTask.ContinueWith((task, result) =>
            {
                Device.BeginInvokeOnMainThread(onFinish);
                UserDialogs.Instance.HideLoading();

                if (result == null)
                    AppProvider.PopUpFactory.ShowMessage(AppResources.FailedServer,
                        AppResources.Error);
            });
            _backgroundWorkers[AppBackgroundWorkerType.SearchExhibitors].Add(searchTask);
        }

        public void DownloadAllUsers(Action onFinish = null)
        {
            var parameters = new KinveyDownloadListParameters(KinveyDownloadCountType.All,
                QueryBuilder.Instance.GetUsersSortByNameKinveyQuery());

            var downloadAllUsersTask = new DownloadUsersBackgroundTask(AppModel.Instance.Users,
                parameters);
            downloadAllUsersTask.ContinueWith((task, result) => Device.BeginInvokeOnMainThread(onFinish));

            _backgroundWorkers[AppBackgroundWorkerType.DownloadUsers].Add(downloadAllUsersTask);
        }

        public void SearchUsers(DynamicListData<User> dataModel, string pattern, Action onFinish)
        {
            String searchPatern = FirstCharToUpper(pattern);
            //TODO reativar Search user AppController
            var parameters = new KinveyDownloadListParameters(KinveyDownloadCountType.All,
                 QueryBuilder.Instance.GetSearchUsersKinveyQuery(searchPatern));

            var searchTask = new DownloadUsersBackgroundTask(dataModel, parameters);
            searchTask.ContinueWith((task, result) =>
            {
                Device.BeginInvokeOnMainThread(onFinish);
                if (result == null)
                    AppProvider.PopUpFactory.ShowMessage(AppResources.FailedServer, AppResources.Error);
            });
            _backgroundWorkers[AppBackgroundWorkerType.SearchUsers].Add(searchTask);
        }

        public void UpdateRating(DynamicListData<User> users, Action onFinish)
        {
            /* TODO reativar ranking AppController
            UserDialogs.Instance.ShowLoading(AppResources.LoadingRanking);

            var parameters = new DownloadListParameters(DownloadCountType.FirstPage,
                                 QueryBuilder.Instance.GetRankingQuery());

            var requestTask = new DownloadUsersBackgroundTask(AppModel.Instance.Users, parameters);
            requestTask.ContinueWith((task, result) =>
            {
                UserDialogs.Instance.HideLoading();

                if (result == null)
                    AppProvider.PopUpFactory.ShowMessage(AppResources.FailedServer, AppResources.Error);
                else
                {
                    users.ClearData();
                    users.UpdateData(result);

                    Device.BeginInvokeOnMainThread(onFinish);
                }
            });
            _backgroundWorkers[AppBackgroundWorkerType.DownloadRanking].Add(requestTask);
            */
        }

        public void TryResetPassword()
        {
            UserDialogs.Instance.PromptAsync(new PromptConfig
            {
                Title = AppResources.LoginForgetPassword,
                Placeholder = AppResources.LoginForgetPasswordEnterEmail,
                IsCancellable = true
            }).ContinueWith(task =>
            {
                PromptResult result = task.Result;

                if (result.Ok)
                    AppController.Instance.ResetForgetPassword(result.Text);
            });
        }

        public void DownloadWallPostLikes(DynamicListData<WallPostLike> wallPostLikes, WallPost wallPost, Action onFinish)
        {
            var requestTask = new DownloadWallPostLikesBackgroundTask(wallPost, wallPostLikes);
            requestTask.ContinueWith((task, result) => Device.BeginInvokeOnMainThread(onFinish));
            _backgroundWorkers[AppBackgroundWorkerType.DownloadWallPostLikes].Add(requestTask);
        }

        public void ResetForgetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                AppProvider.PopUpFactory.ShowMessage(AppResources.ResetPasswordIsEmpty, AppResources.Warning);
            }
            else
            {
                Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.ShowLoading(AppResources.LoadingResetPassword));

                var resetTask = new ResetPasswordBackgroundTask(email);
                resetTask.ContinueWith((task, result) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        UserDialogs.Instance.HideLoading();

                        if (task.Exception != null)
                        {
                            ServerException exception = (ServerException)task.Exception.InnerException;

                            if (exception != null && !string.IsNullOrEmpty(exception.ErrorMessage))
                            {
                                var serverError = JsonConvert.DeserializeObject<ServerError>(exception.ErrorMessage);
                                AppProvider.PopUpFactory.ShowMessage(serverError.ErrorMessage, AppResources.Warning);
                            }
                            else
                                AppProvider.PopUpFactory.ShowMessage(AppResources.FailedServer, AppResources.Warning);
                        }
                        else
                        {
                            if (result != null)
                                UserDialogs.Instance.ShowSuccess(AppResources.SuccessfulResetPassword, 1);
                            else
                                AppProvider.PopUpFactory.ShowMessage(AppResources.FailedServer, AppResources.Warning);
                        }
                    });
                });
                _backgroundWorkers[AppBackgroundWorkerType.DefaultApp].Add(resetTask);
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

        public void Dispose()
        {
        }

        public void AddImage(string fakeImagePath, Action onExecuted, int cropSize)
        {
            var config = new ActionSheetConfig();
//            config.Add(AppResources.TakePicture, () => AddImageAction(fakeImagePath, onExecuted, cropSize, true));
            config.Add(AppResources.UploadImageFromGallery, () => AddImageAction(fakeImagePath, onExecuted, cropSize, false));
            config.Add(AppResources.Cancel);
            UserDialogs.Instance.ActionSheet(config);
        }
        /*
        private void stopForMilliSeconds(int millisecondsToWait)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (true)
            {
                //some other processing to do possible
                if (stopwatch.ElapsedMilliseconds >= millisecondsToWait)
                {
                    break;
                }
            }
        }

        private void testPath(String foto)
        {
            AppProvider.PopUpFactory.ShowMessage(foto, "teste");
            if (AppProvider.IOManager.FileExists(foto))
            {
                AppProvider.PopUpFactory.ShowMessage(foto, "ok");
            }
            else
            {
                AppProvider.PopUpFactory.ShowMessage("merde2", "arf");
            }
        }

        private void SavePhoto(CameraMediaStorageOptions cmso, string fakeImagePath, Action onExecuted, int cropSize)
        {
            //ImageSource imgSource = null;
            String imgpath = "";

            Task<MediaFile> imageTask = AppProvider.MediaPicker.TakePhotoAsync(cmso);

            imageTask.ContinueWith(t =>
             {
                 if (t.IsFaulted)
                 {
                     var s = t.Exception.InnerException.ToString();
                 }
                 else
                 {
                     var mediaFile = t.Result;
                     imgpath = mediaFile.Path;
                     if (AppProvider.IOManager.FileExists(imgpath))
                     {
                         AppProvider.PopUpFactory.ShowMessage(mediaFile.Path, "ok");
                         AppProvider.IOManager.DeleteFile(fakeImagePath);
                         AppProvider.ImageService.CropAndResizeImage(imgpath, fakeImagePath, cropSize);
                         onExecuted.Invoke();
                     }
                     else
                     {
                         AppProvider.PopUpFactory.ShowMessage("merde1", "ok");
                     }
                 }
             });
        }
        */

        private void AddImageAction(string fakeImagePath, Action onExecuted, int cropSize, bool isMakePhoto)
        {
            Task<MediaFile> imageTask = null;
            var options = new CameraMediaStorageOptions
            {
                DefaultCamera = CameraDevice.Rear,
                MaxPixelDimension = 400,
            };

            if (isMakePhoto && AppProvider.MediaPicker.IsCameraAvailable)
                imageTask = AppProvider.MediaPicker.TakePhotoAsync(options);
            else
                imageTask = AppProvider.MediaPicker.SelectPhotoAsync(options);

            imageTask.ContinueWith(delegate (Task<MediaFile> arg) {
                MediaFile file = arg.Result;

                if (file != null)
                {
                    AppProvider.IOManager.DeleteFile(fakeImagePath);
                    AppProvider.ImageService.CropAndResizeImage(file.Path, fakeImagePath, cropSize);

                    onExecuted.Invoke();
                }
            });
        }


        private void ShowServerError()
        {
            AppProvider.PopUpFactory.ShowMessage(AppResources.FailedServer, AppResources.Error);
        }

        public void UpdatePushNotifications(string deviceToken, DeviceType deviceType)
        {
            if (string.IsNullOrEmpty(AppModel.Instance.AppInformation.PushNotificationToken))
            {
                AppModel.Instance.UpdatePushNotificationToken(deviceToken, deviceType);
            }
            else if (!deviceToken.Equals(AppModel.Instance.AppInformation.PushNotificationToken))
            {
                UserDialogs.Instance.ShowLoading(AppResources.LoadingPushNotification);

                var updatingPushNotificationTask = new DownloadPushNotificationsByTokenBackgroundTask(AppModel.Instance.AppInformation.PushNotificationToken, AppModel.Instance.AppInformation.PushNotificationPlatform);
                updatingPushNotificationTask.ContinueWith((task, result) =>
                {
                    if (result == null)
                    {
                        UserDialogs.Instance.HideLoading();
                        ShowServerError();
                    }
                    else
                    {
                        if (result.Count > 0)
                        {
                            var deletePushNotificationTask = new DeleteOldPushNotificationsBackgroundTask(result);
                            deletePushNotificationTask.ContinueWith((deleteTask, deleteResult) =>
                            {
                                UserDialogs.Instance.HideLoading();
                                AppModel.Instance.UpdatePushNotificationToken(deviceToken, deviceType);
                            });
                            _backgroundWorkers[AppBackgroundWorkerType.DefaultApp].Add(deletePushNotificationTask);
                        }
                        else
                        {
                            UserDialogs.Instance.HideLoading();
                            AppModel.Instance.UpdatePushNotificationToken(deviceToken, deviceType);
                        }
                    }
                });
                _backgroundWorkers[AppBackgroundWorkerType.DefaultApp].Add(updatingPushNotificationTask);
            }
        }

        public static string FirstCharToUpper(string input)
        {
            String result = " ";
            if (!String.IsNullOrEmpty(input))
            {
                result = input.Substring(0, 1).ToUpper() + input.Substring(1);
            }

            return result;
        }
    }
}