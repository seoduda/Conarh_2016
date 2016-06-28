using Conarh_2016.Application.DataAccess;
using Conarh_2016.Application.Domain;
using Conarh_2016.Core;
using Conarh_2016.Core.Net;
using Conarh_2016.Core.Services;
using Core.Tasks;
using System;
using System.Threading.Tasks;

namespace Conarh_2016.Application.BackgroundTasks.GetData
{
    public class DownloadImageBackgroundTask : OneShotBackgroundTask<ImageData>
    {
        public readonly DateTime UpdatedAtTime;
        public readonly string ImageServerPath;

        public DownloadImageBackgroundTask(DateTime updatedAtTime, string imageServerPath)
        {
            UpdatedAtTime = updatedAtTime;
            ImageServerPath = imageServerPath;
        }

        public override ImageData Execute()
        {
            try
            {
                ImageData imageData = ImageLoader.Instance.GetImageData(ImageServerPath);

                if (imageData == null || imageData.UpdatedAtTime < UpdatedAtTime)
                {
                    Task<byte[]> task = WebClient.GetBytesAsync(ImageServerPath, WebClient.DefaultRequestHeaders);
                    task.ConfigureAwait(false);

                    return SaveImage(ImageServerPath, task.Result, UpdatedAtTime);
                }
                else
                    return imageData;
            }
            catch (Exception ex)
            {
                AppProvider.Log.WriteLine(LogChannel.Exception, ex);
            }

            return null;
        }

        public ImageData SaveImage(string serverPath, byte[] data, DateTime updatedAt)
        {
            if (string.IsNullOrEmpty(serverPath))
                return null;

            var imageData = ImageLoader.Instance.GetImageData(serverPath);

            if (imageData == null || imageData.UpdatedAtTime < updatedAt)
            {
                imageData = imageData ?? new ImageData(serverPath);
                imageData.UpdatedAtTime = updatedAt;
                AppProvider.IOManager.SaveFile(data, imageData.ImagePath);

                AppModel.Instance.Images.AddOne(imageData);

                DbClient.Instance.SaveItemData<ImageData>(imageData).ConfigureAwait(false);
            }

            return imageData;
        }
    }
}