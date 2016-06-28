using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Conarh_2016.Core.Exceptions
{
    public static class ExceptionExtensions
    {
        public static CoreException Wrap(this Exception exception)
        {
            if (exception is CoreException)
                return exception as CoreException;
            if (exception is WebException || exception is TaskCanceledException || exception is TimeoutException)
                return new ServerException(HttpStatusCode.ServiceUnavailable, exception.GetType().ToString(), string.Empty);
            return new UnhandledSystemException(exception);
        }

        public static void ThrowOnHttpError(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                string uri = response.RequestMessage.RequestUri.ToString();

                throw new ServerException(response.StatusCode, content, uri);
            }
        }
    }
}