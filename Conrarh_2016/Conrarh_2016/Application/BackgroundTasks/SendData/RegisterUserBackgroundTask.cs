using Conarh_2016.Application.BackgroundTasks.SendData.Kinvey;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Domain.PostData;
using Conarh_2016.Application.Tools;
using Conarh_2016.Core;
using Conarh_2016.Core.Net;
using Conarh_2016.Core.Services;
using Core.Tasks;
using Newtonsoft.Json;
using System;

namespace Conarh_2016.Application.BackgroundTasks
{
    public sealed class RegisterUserBackgroundTask : OneShotBackgroundTask<CreateUserData>
    {
        public readonly CreateUserData Data;
        public readonly bool IsCreateNew;
        public readonly string UserId;

        public RegisterUserBackgroundTask(CreateUserData data, bool isCreateNew = true, string userId = null)
        {
            Data = data;
            IsCreateNew = isCreateNew;
            UserId = userId;
        }

        public override CreateUserData Execute()
        {
            try
            {
                if (IsCreateNew)
                    Data.ScorePoints = 0;
                string imagePath = " ";
                if (!Data.ProfileImage.ToLower().StartsWith("http"))
                {
                    imagePath = Data.ProfileImage;
                    Data.ProfileImage = null;
                }

                string serializedData = JsonConvert.SerializeObject(Data);

                string userId = UserId;

                if (IsCreateNew)
                {
                    //	var result = WebClient.PostStringAsync(QueryBuilder.Instance.GetUsersQuery (), serializedData).Result;
                    var result = KinveyWebClient.PostSignUpStringAsync(QueryBuilder.Instance.GetUsersKinveyQuery(), serializedData).Result;

                    UniqueItem uniqueItemId = JsonConvert.DeserializeObject<UniqueItem>(result);
                    userId = uniqueItemId.Id;
                }
                else
                {
                    if (!Data.IsEmpty())
                    {
                        AppProvider.Log.WriteLine(LogChannel.All, QueryBuilder.Instance.GetPostUserProfileChangesKinveyQuery(UserId) + " : " + serializedData);
                        var update = KinveyWebClient.PutStringAsync(QueryBuilder.Instance.GetPostUserProfileChangesKinveyQuery(UserId), serializedData).Result;
                    }
                }

                if (!string.IsNullOrEmpty(imagePath.Trim()))
                {
                    var postImageToPost = new PostImageKinveyBackgroundTask(imagePath, userId, KinveyImageType.User);

                    postImageToPost.Execute();
                    //Data.ProfileImage = postImageToPost.Execute();

                    //var response = WebClient.PutBytesAsync(QueryBuilder.Instance.GetUploadUserImageQuery (userId), imageContent).Result;
                }

                return Data;
            }
            catch (Exception ex)
            {
                AppProvider.Log.WriteLine(LogChannel.Exception, ex);
                Exception = ex;
            }

            return null;
        }
    }
}