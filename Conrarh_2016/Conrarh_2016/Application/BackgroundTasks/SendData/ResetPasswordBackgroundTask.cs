using System;
using Conarh_2016.Application.Domain.PostData;
using Conarh_2016.Application.Tools;
using Conarh_2016.Core;
using Conarh_2016.Core.Net;
using Newtonsoft.Json;
using Core.Tasks;

namespace Conarh_2016.Application.BackgroundTasks
{
	public sealed class ResetPasswordBackgroundTask : OneShotBackgroundTask<string>
	{
		public readonly string Email;
		public ResetPasswordBackgroundTask(string email)
		{
			Email = email;
		}

		public override string Execute ()
		{
			string sessionUri = QueryBuilder.Instance.GetPostResetPasswordKinveyQuery(Email);
			try
			{
                string result = KinveyWebClient.PostSignUpStringAsync(sessionUri, "").Result;

                return result;
			}
			catch(Exception ex)
			{
				Exception = ex;
				AppProvider.Log.WriteLine (Conarh_2016.Core.Services.LogChannel.Exception, ex);
			}

			return null;
		}
	}
}