
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
    public static class WebClient
    {
        internal const string XApiToken = "";
        internal const string JsonMimeType = "application/json";

        public static Dictionary<string, string> DefaultRequestHeaders = new Dictionary<string, string> {
            {"X-Api-Token", "Vwb0wQamaGte+nfAr6+vQgMEBkoTJK5Sz0K/d4+yoO6gz2MJmG8G/VrnFbGbhjaTN1c2onLzYGKU3h9LghYF8w=="},
            {"Accept-Charset", Encoding.UTF8.WebName }
        };

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
            }, DefaultRequestHeaders, acceptedContentType, timeout).ConfigureAwait(false);
        }

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
            }, DefaultRequestHeaders, acceptedContentType, timeout).ConfigureAwait(false);
        }

        public static async Task<string> GetStringAsync(string requestUri, string acceptedContentType = JsonMimeType,
            TimeSpan timeout = default(TimeSpan), CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteStringQuery(async client => await client.GetAsync(requestUri, cancellationToken).ConfigureAwait(false),
                DefaultRequestHeaders, acceptedContentType, timeout).ConfigureAwait(false);
        }

        public static async Task<byte[]> GetBytesAsync(string requestUri, Dictionary<string, string> defaultRequestHeaders, TimeSpan timeout = default(TimeSpan), CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteByteQuery(async client => await client.GetAsync(requestUri, cancellationToken).ConfigureAwait(false),
                defaultRequestHeaders, string.Empty, timeout).ConfigureAwait(false);
        }

        public static async Task<string> DeleteAsync(string requestUri, TimeSpan timeout = default(TimeSpan), CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteStringQuery(async client => await client.DeleteAsync(requestUri, cancellationToken).ConfigureAwait(false), DefaultRequestHeaders, string.Empty, timeout).ConfigureAwait(false);
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
        {
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