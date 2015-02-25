using System;

namespace SantaHo.Core.Exceptions
{
    public class StartupConfigurationException : Exception
    {
        public StartupConfigurationException()
        {
        }

        public StartupConfigurationException(string message) : base(message)
        {
        }

        public StartupConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
