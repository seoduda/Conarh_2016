using System;

namespace Conarh_2016.Core.Exceptions
{
    public abstract class CoreException : Exception
    {
        public string ErrorMessage { protected set; get; }

        protected CoreException(string errorMessage = null, Exception innerException = null) : base(errorMessage, innerException)
        {
            ErrorMessage = errorMessage ?? "Generic exception occured";
        }

        public override string ToString()
        {
            return string.Format("{0}: Message: {1}", GetType(), ErrorMessage);
        }
    }
}