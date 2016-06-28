using System;
using Conarh_2016.Application.Domain.PostData;
using Conarh_2016.Application.Tools;
using Conarh_2016.Core;
using Conarh_2016.Core.Net;
using Conarh_2016.Core.Services;
using Newtonsoft.Json;
using Core.Tasks;

namespace Conarh_2016.Application.BackgroundTasks
{
	public sealed class PostImageBackgroundTask : OneShotBackgroundTask<string>
	{
		public readonly string ImagePath;
		public readonly string Query;

		public PostImageBackgroundTask(string query, string imagePath)
		{
			Query = query;
			ImagePath = imagePath;
		}

		public override string Execute ()
		{
			try
			{
				var imageContent = AppProvider.IOManager.GetBytesFileContent(ImagePath);
				var response = WebClient.PutBytesAsync(Query, imageContent).Result;

				return response;
			}
			catch(Exception ex) 
			{
				AppProvider.Log.WriteLine (LogChannel.Exception, ex);
				Exception = ex;
			}

			return null;
		}
	}
}