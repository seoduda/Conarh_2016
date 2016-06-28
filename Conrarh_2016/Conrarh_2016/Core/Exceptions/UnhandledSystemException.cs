using System;

namespace Conarh_2016.Core.Exceptions
{
    internal class UnhandledSystemException : CoreException
    {
        public UnhandledSystemException(Exception innerException) : base(innerException: innerException)
        {
        }
    }
}