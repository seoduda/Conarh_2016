using System.Net;

namespace Conarh_2016.Core.Exceptions
{
    internal class ServerException : CoreException
    {
        public HttpStatusCode StatusCode { get; protected set; }

        public string Uri { get; protected set; }

        public ServerException(HttpStatusCode statusCode, string messageError, string uri) : base(messageError)
        {
            StatusCode = statusCode;
            Uri = uri;
        }

        public override string ToString()
        {
            return string.Format("{0}: Code: {1} Message: {2}, Uri: {3}", GetType(), StatusCode, ErrorMessage, Uri);
        }
    }
}