using System;
using System.Threading;
using GoodBoy.Bot.Properties;
using Nelibur.ServiceModel.Clients;
using NLog;
using SantaHo.FrontEnd.ServiceContracts.Letters;
using SantaHo.FrontEnd.ServiceContracts.Monitoring;

namespace GoodBoy.Bot.Clients
{
    public sealed class SantaPostOfficeClient : ISantaPostOfficeClient
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private long _sentLetters;
        private readonly JsonServiceClient _client;

        public SantaPostOfficeClient()
        {
            var address = Settings.Default.SantaPostOfficeAddress;
            _client = new JsonServiceClient(address);
        }

        public bool Send(WishListLetterRequest request)
        {
            try
            {
                _client.Post(request);
                Stats();
                return true;
            }
            catch (Exception e)
            {
                Logger.Warn(e);
                return false;
            }
        }

        public bool Hartbeat()
        {
            try
            {
                _client.Post(new HartbeatRequest());
                return true;
            }
            catch (Exception e)
            {
                Logger.Warn(e);
                return false;
            }
        }

        private void Stats()
        {
            var number = Interlocked.Increment(ref _sentLetters);
            if (number % 100 == 0)
            {
                Logger.Debug("({1}) Sent \t#{0}", number, Environment.CurrentManagedThreadId);
            }
        }
    }
}
