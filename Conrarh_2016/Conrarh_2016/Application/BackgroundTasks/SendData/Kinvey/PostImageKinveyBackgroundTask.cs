using Conarh_2016.Core;
using Conarh_2016.Core.Net;
using Core.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Conarh_2016.Application.BackgroundTasks.SendData.Kinvey
{
    public enum KinveyImageType
    {
        User,
        WallPost
    }

    public sealed class KinveyPostImage
    {
        public string _filename;
        public long size;
        public string mimeType;
        public bool _public;
    }

    public sealed class KinveyPostResult
    {
        public string _id;
        public string _filename;
        public int size;
        public string mimeType;
        public string _uploadURL;
        public string _downloadURL;
        public string _expiresAt;
        public Dictionary<string, string> _requiredHeaders;
    }
    /* TODO - implementar classe PostImageKinveyBackgroundTask como : OneShotBackgroundTask */
   //public sealed class PostImageKinveyBackgroundTask : OneShotBackgroundTask<string>
   public sealed class PostImageKinveyBackgroundTask : OneShotBackgroundTask<string>
    {
       public readonly KinveyImageType ImageType;
       public readonly string ImagePath;
       public readonly string Id;

       public PostImageKinveyBackgroundTask(string imagePath, string id, KinveyImageType imageType)
       {
           ImageType = imageType;
           Id = id;
           ImagePath = imagePath;
       }

       public override string Execute()
       {
           string result = null;
           string serverImagePath = "";
           string filename = getImageName(ImageType, Id);
           byte[] imageContent = AppProvider.IOManager.GetBytesFileContent(ImagePath);

           KinveyPostImage kpi = new KinveyPostImage()
           {
               _filename = filename,
               mimeType = "image/jpg",
               _public = true,
               size = AppProvider.IOManager.GetFileSize(ImagePath)
           };
           Dictionary<string, string> ImageRequestHeaders = new Dictionary<string, string>();

           string serializedData = JsonConvert.SerializeObject(kpi);
           result = KinveyWebClient.PostImageStringAsync(serializedData).Result;

           KinveyPostResult kpr = JsonConvert.DeserializeObject<KinveyPostResult>(result);

           foreach (KeyValuePair<string, string> pair in kpr._requiredHeaders)
           {
               ImageRequestHeaders.Add(pair.Key, pair.Value);
           }


           /* TODO - Mudar o HttpClient abaixo para uma função d0 KinveyWebClient */
             HttpClient httpClient = KinveyWebClient.CreateHttpClient(ImageRequestHeaders);
            ByteArrayContent bac = new ByteArrayContent(imageContent);
            bac.Headers.ContentLength = kpi.size;

          
            HttpResponseMessage response = httpClient.PutAsync(kpr._uploadURL, bac).Result;
                ; 
            if (response.IsSuccessStatusCode)
            {
                //string content =  response.Content.ReadAsStringAsync().Result;
                serverImagePath = GetImageDownloadUrl(kpr._id);
            }

            return serverImagePath;
        }

        private string getImageName(KinveyImageType imageType, String Id)
        {
            StringBuilder sBuilder = new StringBuilder();
            switch (imageType)
            {
                case KinveyImageType.User:
                    sBuilder.Append("user_");
                    break;

                case KinveyImageType.WallPost:
                    sBuilder.Append("wall_");
                    break;
            }
            sBuilder.Append(Id);
            sBuilder.Append(".jpg");

            return sBuilder.ToString();
        }

        public String GetImageDownloadUrl(String imageServerId)
        {
            StringBuilder sBuilder = new StringBuilder(KinveyWebClient.KinveyApiBlobUrl);
            sBuilder.Append("/");
            sBuilder.Append(imageServerId);
            String requestUri = sBuilder.ToString();
            String result = null;
            result = KinveyWebClient.GetImageStringAsync(requestUri).Result;
            KinveyPostResult kpr = JsonConvert.DeserializeObject<KinveyPostResult>(result);
            return kpr._downloadURL;
        }

        /*
        public override string Execute()
        {
            throw new NotImplementedException();
        }
        */
    }
}