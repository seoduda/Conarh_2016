using System;
using Newtonsoft.Json;
using Conarh_2016.Application.Domain.PostData;
using Core.Tasks;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Tools;

using Conarh_2016.Core;
using Conarh_2016.Core.Net;
using Conarh_2016.Application.DataAccess;

namespace Conarh_2016.Application.BackgroundTasks
{
    public sealed class LoginBackgroundTask : OneShotBackgroundTask<User>
	{
		public readonly LoginUserData LoginData;

		public LoginBackgroundTask(LoginUserData data)
		{
			LoginData = data;
		}

		public override User Execute ()
		{
			string dataToSerialize = JsonConvert.SerializeObject (LoginData);
			string loginUri = QueryBuilder.Instance.GetPostLoginUserKinveyQuery();
			try
			{
                string result = KinveyWebClient.PostSignUpStringAsync(loginUri, dataToSerialize).Result;
                //string result = WebClient.PostStringAsync(sessionUri, dataToSerialize).Result;

                //LoginUserData logginedData = JsonConvert.DeserializeObject<LoginUserData> (result);
                User _usr = JsonConvert.DeserializeObject<User>(result);
                /*

                string getUserDataUri = QueryBuilder.Instance.GetUserByUsernameQuery (logginedData.Email);
				RootListData<User> rootData = WebClient.GetObjectAsync<RootListData<User>> (getUserDataUri).Result;
                

				AppModel.Instance.Users.AddOne(rootData.Data[0]);
				DbClient.Instance.SaveItemData<User>(rootData.Data[0]).ConfigureAwait(false);

				return rootData.Data[0];
                */

                AppModel.Instance.Users.AddOne(_usr);
                DbClient.Instance.SaveItemData<User>(_usr).ConfigureAwait(false);

                return _usr;

            }
            catch (Exception ex)
			{
				Exception = ex;
				AppProvider.Log.WriteLine (Conarh_2016.Core.Services.LogChannel.Exception, ex);
			}

			return null;
		}
	}
}