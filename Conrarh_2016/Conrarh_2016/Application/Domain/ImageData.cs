
using Conarh_2016.Core;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Conarh_2016.Application.Domain
{
    public sealed class ImageData : UpdatedUniqueItem
    {
        [JsonProperty(JsonKeys.ServerPath)]
        public string ServerPath
        {
            set;
            get;
        }

        public string ImagePath
        {
            get
            {
                return Path.Combine(AppProvider.IOManager.DocumentPath, "Images", string.Format("{0}.jpeg", Id));
            }
        }

        public ImageData(string serverPath)
        {
            Id = Guid.NewGuid().ToString();
            ServerPath = serverPath;
        }

        public ImageData()
        {

        }

        public override void UpdateWithItem(UniqueItem item)
        {
            ImageData image = item as ImageData;

            if (image != null)
            {
                UpdatedAtTime = image.UpdatedAtTime;
            }
        }

        public new static class JsonKeys
        {
            public const string ServerPath = "serverPath";
            public const string Id = "id";
        }


    }
}
