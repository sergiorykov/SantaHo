using System;

namespace SantaHo.Core.Exceptions
{
    public class QueueUnavailableException : Exception
    {
        public QueueUnavailableException()
        {
        }

        public QueueUnavailableException(string message) : base(message)
        {
        }

        public QueueUnavailableException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
