using System;
using NLog;
using SantaHo.Core.ApplicationServices;
using SantaHo.Core.Exceptions;

namespace SantaHo.FrontEnd.Service.Queues
{
    public sealed class QueueSettings
    {
        private const string RabbitMqAmqpUriKey = "RabbitMQ:AmqpUri";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public string RabbitAmqpUri { get; set; }

        public static QueueSettings Create(IStartupSettings settings)
        {
            try
            {
                return new QueueSettings
                {
                    RabbitAmqpUri = settings.GetValue<string>(RabbitMqAmqpUriKey)
                };
            }
            catch (Exception e)
            {
                Logger.Error("Loading settings failed", e);
                throw new StartupConfigurationException(string.Format("Startup Settings corrupted: {0}", RabbitMqAmqpUriKey));
            }
        }
    }
}