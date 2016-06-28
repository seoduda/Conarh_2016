
using Conarh_2016.Application.Domain;
using Conarh_2016.Core;
using Conarh_2016.Application.BackgroundTasks.GetData;
using Core.Tasks;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Conarh_2016.Application
{
    public sealed class ImageLoader : Singletone<ImageLoader>
    {
        private readonly BackgroundWorker _imageLoader;

        public ImageLoader()
        {
            _imageLoader = new BackgroundWorker();
            _imageLoader.Start();
        }

        public void LoadImage(string imageServerPath, DateTime updatedAt, Action<ImageData> onImageLoaded)
        {
            ImageData data = GetImageData(imageServerPath);

            if (data == null || data.UpdatedAtTime < updatedAt)
            {
                var downloadTask = new DownloadImageBackgroundTask(updatedAt, imageServerPath);
                downloadTask.ContinueWith((task, result) => {

                    if (result != null)
                        onImageLoaded.Invoke(result);
                });

                _imageLoader.Add(downloadTask);
            }
            else
                onImageLoaded.Invoke(data);
        }

        public ImageData GetImageData(string serverPath)
        {
            return AppModel.Instance.Images.Items.Find(temp => temp.ServerPath.Equals(serverPath));
        }

        private Dictionary<string, ImageSource> images = new Dictionary<string, ImageSource>();
        public ImageSource GetImage(string path, bool saveForReuse)
        {
            if (!saveForReuse)
                return ImageSource.FromFile(path);

            if (!images.ContainsKey(path))
                images.Add(path, ImageSource.FromFile(path));

            return images[path];
        }
    }
}
