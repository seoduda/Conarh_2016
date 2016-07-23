using Conarh_2016.Application;
using Conarh_2016.Core.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Conarh_2016.Core.Net
{
    public static class KinveyWebClient
    {
        internal const string APiToken = "Basic ZWR1YXJkb0BvdXRsb29rLmNvbTpwYW5hbWE=";
        internal const string AppCredential = "Basic a2lkX1MxdU1WRlN2OjgyZTU4YzMzZWUwODQzMTQ5ZGI1MzI1ZmRlNGIxMTcw";

        internal const string JsonMimeType = "application/json";
        internal const string KinveyApiBlobUrl = "https://baas.kinvey.com/blob/kid_S1uMVFSv";

        public static Dictionary<string, string> DefaultRequestHeaders = new Dictionary<string, string> {
            //{"Authorization", GetAPiToken()},
            {"Authorization", APiToken},
            {"Charset", Encoding.UTF8.WebName }
        };

        public static Dictionary<string, string> AppCredentialHeaders = new Dictionary<string, string> {
            {"Authorization", AppCredential},
            {"Charset", Encoding.UTF8.WebName }
        };

        public static Dictionary<string, string> ImageRequestHeaders = new Dictionary<string, string> {
            //{"Authorization", GetAPiToken()},
            {"Authorization", APiToken},
            {"X-Kinvey-API-Version", "3" }
        };

        /* TODO - mudar para a authkey de cada usuário 
        private static String GetAuthKey(bool Appcredential)
        {
            String s = null;
            String key = Appcredential ? "Basic a2lkX1MxdU1WRlN2OjgyZTU4YzMzZWUwODQzMTQ5ZGI1MzI1ZmRlNGIxMTcw" : GetUserAPiToken();
            return key;
        }


        private static string GetUserAPiToken()
        {   if (AppModel.Instance.CurrentUser == null)
            {
                return "Basic ZWR1YXJkb0BvdXRsb29rLmNvbTpwYW5hbWE=";
            }
            else
            {
                StringBuilder sb = new StringBuilder("Basic ");
                sb.Append(getKinveyAuthString());
                return sb.ToString();
            }
        }

        private static string getKinveyAuthString()
        {
            StringBuilder sb = new StringBuilder(AppModel.Instance.CurrentUser.User.Email);
            sb.Append(":");
            sb.Append(AppModel.Instance.AppInformation.CurrentUserPassword);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            return System.Convert.ToBase64String(plainTextBytes);
        }
        */
        public static async Task<string> DeleteStringAsync(string requestUri, string acceptedContentType = JsonMimeType,
     TimeSpan timeout = default(TimeSpan), CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteStringQuery(async client => await client.DeleteAsync(requestUri, cancellationToken).ConfigureAwait(false),
                    DefaultRequestHeaders, acceptedContentType, timeout).ConfigureAwait(false);
        }

        public static async Task<T> GetObjectAsync<T>(string requestUri, TimeSpan timeout = default(TimeSpan),
            CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            string result = await GetStringAsync(requestUri, JsonMimeType, timeout, cancellationToken).ConfigureAwait(false) ?? String.Empty;
            return JsonConvert.DeserializeObject<T>(result);
        }

        public static async Task<T> PutObjectAsync<T>(string requestUri, object data, string contentType = JsonMimeType, string acceptedContentType = JsonMimeType,
            TimeSpan timeout = default(TimeSpan), CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            string result = await PutStringAsync(requestUri, JsonConvert.SerializeObject(data), JsonMimeType, JsonMimeType, timeout, cancellationToken).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(result);
        }

        public static async Task<string> PutStringAsync(string requestUri, string data, string contentType = JsonMimeType, string acceptedContentType = JsonMimeType,
            TimeSpan timeout = default(TimeSpan), CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteStringQuery(async client =>
            {
                using (var stringContent = new StringContent(data) { Headers = { ContentType = new MediaTypeHeaderValue(contentType) } })
                    return await client.PutAsync(requestUri, stringContent, cancellationToken).ConfigureAwait(false);
            }, DefaultRequestHeaders, acceptedContentType, timeout).ConfigureAwait(false);
        }

        public static async Task<string> PutBytesAsync(string requestUri, byte[] data, string acceptedContentType = JsonMimeType,
            TimeSpan timeout = default(TimeSpan), CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteStringQuery(async client =>
            {
                using (var content = new ByteArrayContent(data) { Headers = { ContentType = new MediaTypeHeaderValue("image/jpeg") } })
                {
                    return await client.PutAsync(requestUri, content, cancellationToken).ConfigureAwait(false);
                }
            }, DefaultRequestHeaders,  acceptedContentType, timeout).ConfigureAwait(false);
        }

        /*
        public static async Task<string> PutImageBytesAsync(string requestUri, byte[] data, Dictionary<string, string> RequestHeaders, string acceptedContentType = JsonMimeType,
            TimeSpan timeout = default(TimeSpan), CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteStringQuery(async client =>
            {
                using (var content = new ByteArrayContent(data) { Headers = { ContentType = new MediaTypeHeaderValue("image/jpeg") } })
                {
                    return await client.PutAsync(requestUri, content, cancellationToken).ConfigureAwait(false);
                }
            }, RequestHeaders, acceptedContentType, timeout).ConfigureAwait(false);
        }
        */

        public static async Task<T> PostObjectAsync<T>(string requestUri, object data, TimeSpan timeout = default(TimeSpan), CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            string result = await PostObjectAsync(requestUri, data, JsonMimeType, JsonMimeType, timeout, cancellationToken).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(result);
        }

        public static async Task<string> PostObjectAsync(string requestUri, object data, string contentType = JsonMimeType, string acceptedContentType = JsonMimeType,
            TimeSpan timeout = default(TimeSpan), CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PostStringAsync(requestUri, JsonConvert.SerializeObject(data), JsonMimeType, JsonMimeType, timeout, cancellationToken).ConfigureAwait(false);
        }

        public static async Task<string> PostStringAsync(string requestUri, string data, string contentType = JsonMimeType, string acceptedContentType = JsonMimeType,
            TimeSpan timeout = default(TimeSpan), CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteStringQuery(async client =>
            {
                using (var stringContent = new StringContent(data) { Headers = { ContentType = new MediaTypeHeaderValue(contentType) } })
                {
                    return await client.PostAsync(requestUri, stringContent, cancellationToken).ConfigureAwait(false);
                }
            }, DefaultRequestHeaders,  acceptedContentType, timeout).ConfigureAwait(false);
        }

        public static async Task<string> PostImageStringAsync(string data, string contentType = JsonMimeType, string acceptedContentType = JsonMimeType,
                TimeSpan timeout = default(TimeSpan), CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteStringQuery(async client =>
            {
                using (var stringContent = new StringContent(data) { Headers = { ContentType = new MediaTypeHeaderValue(contentType) } })
                {
                    return await client.PostAsync(KinveyApiBlobUrl, stringContent, cancellationToken).ConfigureAwait(false);
                }
            }, DefaultRequestHeaders,   acceptedContentType, timeout).ConfigureAwait(false);
        }

        public static async Task<string> PostSignUpStringAsync(string requestUri, string data, string contentType = JsonMimeType, string acceptedContentType = JsonMimeType,
            TimeSpan timeout = default(TimeSpan), CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteStringQuery(async client =>
            {
                using (var stringContent = new StringContent(data) { Headers = { ContentType = new MediaTypeHeaderValue(contentType) } })
                {
                    return await client.PostAsync(requestUri, stringContent, cancellationToken).ConfigureAwait(false);
                }
            }, AppCredentialHeaders, acceptedContentType, timeout).ConfigureAwait(false);
        }

        public static async Task<string> GetStringAsync(string requestUri, string acceptedContentType = JsonMimeType,
            TimeSpan timeout = default(TimeSpan), CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteStringQuery(async client => await client.GetAsync(requestUri, cancellationToken).ConfigureAwait(false),
                DefaultRequestHeaders, acceptedContentType, timeout).ConfigureAwait(false);
        }

        public static async Task<string> GetImageStringAsync(string requestUri, string acceptedContentType = JsonMimeType,
            TimeSpan timeout = default(TimeSpan), CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteStringQuery(async client => await client.GetAsync(requestUri, cancellationToken).ConfigureAwait(false),
                ImageRequestHeaders, acceptedContentType, timeout).ConfigureAwait(false);
        }

        public static async Task<byte[]> GetBytesAsync(string requestUri, Dictionary<string, string> defaultRequestHeaders, TimeSpan timeout = default(TimeSpan), CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteByteQuery(async client => await client.GetAsync(requestUri, cancellationToken).ConfigureAwait(false),
                defaultRequestHeaders, string.Empty, timeout).ConfigureAwait(false);
        }

        public static async Task<string> DeleteAsync(string requestUri, TimeSpan timeout = default(TimeSpan), CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteStringQuery(async client => await client.DeleteAsync(requestUri, cancellationToken).ConfigureAwait(false), DefaultRequestHeaders,  string.Empty, timeout).ConfigureAwait(false);
        }

        private static async Task<string> ExecuteStringQuery(Func<HttpClient, Task<HttpResponseMessage>> requestTask, Dictionary<string, string> defaultRequestHeaders, string acceptedContentType, TimeSpan timeout)
        {
            return await ExecuteQuery(requestTask, async content => await content.ReadAsStringAsync().ConfigureAwait(false),
                defaultRequestHeaders, acceptedContentType, timeout).ConfigureAwait(false);
        }

        private static async Task<byte[]> ExecuteByteQuery(Func<HttpClient, Task<HttpResponseMessage>> requestTask, Dictionary<string, string> defaultRequestHeaders, string acceptedContentType, TimeSpan timeout)
        {
            return await ExecuteQuery(requestTask, async content => await content.ReadAsByteArrayAsync().ConfigureAwait(false),
                defaultRequestHeaders, acceptedContentType, timeout).ConfigureAwait(false);
        }

        private static async Task<T> ExecuteQuery<T>(Func<HttpClient, Task<HttpResponseMessage>> doRequestTask,
              Func<HttpContent, Task<T>> readResponceTask, Dictionary<string, string> defaultRequestHeaders, string acceptedContentType, TimeSpan timeout)
        //Func<HttpContent, Task<T>> readResponceTask, Dictionary<string, string> defaultRequestHeaders, bool AppCredentials,  string acceptedContentType, TimeSpan timeout)
        {
            /*
            if (defaultRequestHeaders.ContainsKey("Authorization"))
            {
                defaultRequestHeaders.Remove("Authorization");
                defaultRequestHeaders.Add("Authorization", GetAuthKey(AppCredentials));
            }
            */
            try
            {
                using (HttpClient httpClient = CreateHttpClient(defaultRequestHeaders, acceptedContentType, timeout))
                using (HttpResponseMessage responseMessage = await doRequestTask(httpClient).ConfigureAwait(false))
                using (responseMessage.RequestMessage)
                {
                    responseMessage.ThrowOnHttpError();
                    return await readResponceTask(responseMessage.Content).ConfigureAwait(false);
                }
            }
            catch (Exception exception)
            {
                if (exception is CoreException)
                    throw;
                throw exception.Wrap();
            }
        }

        public static HttpClient CreateHttpClient(Dictionary<string, string> defaultRequestHeaders, string acceptedContentType = null, TimeSpan timeout = default(TimeSpan))
        {
            var handler = new HttpClientHandler();

            var client = new HttpClient(handler, true);

            if (timeout != default(TimeSpan))
                client.Timeout = timeout;

            if (!String.IsNullOrEmpty(acceptedContentType))
                client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(acceptedContentType));

            foreach (string header in defaultRequestHeaders.Keys)
                client.DefaultRequestHeaders.Add(header, defaultRequestHeaders[header]);

            return client;
        }
        
    }
}