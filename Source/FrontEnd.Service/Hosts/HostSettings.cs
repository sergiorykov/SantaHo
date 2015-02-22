using System;
using NLog;
using SantaHo.Core.ApplicationServices;
using SantaHo.Core.Exceptions;

namespace SantaHo.FrontEnd.Service.Hosts
{
    public sealed class HostSettings
    {
        private const string ServiceServiceHostUriKey = "ServiceHost:Uri";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public Uri ServiceHostUri { get; set; }

        public static HostSettings Create(IStartupSettings settings)
        {
            try
            {
                return new HostSettings
                {
                    ServiceHostUri = new Uri(settings.GetValue<string>(ServiceServiceHostUriKey))
                };
            }
            catch (Exception e)
            {
                Logger.Error("Loading settings failed", e);
                throw new StartupConfigurationException(string.Format("Startup Settings corrupted: {0}",
                    ServiceServiceHostUriKey));
            }
        }
    }
}