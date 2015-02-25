using System;

namespace SantaHo.Infrastructure.Core.Constants
{
    public static class QueueKeys
    {
        public static class IncomingLetters
        {
            public const string ExchangeName = "incoming-letters-direct-exchange";
            public const string QueueName = "incoming-letters";
            public const string RoutingKey = "letter";
        }
    }
}
