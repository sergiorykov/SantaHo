using System;
using System.Threading;
using GoodBoy.Bot.Properties;
using Nelibur.ServiceModel.Clients;
using NLog;
using SantaHo.ServiceContracts.Letters;
using SantaHo.ServiceContracts.Monitoring;

namespace GoodBoy.Bot.Clients
{
    public sealed class SantaPostOfficeClient : ISantaPostOfficeClient
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly JsonServiceClient _client;
        private long _sentLetters;

        public SantaPostOfficeClient()
        {
            string address = Settings.Default.SantaPostOfficeAddress;
            _client = new JsonServiceClient(address);
        }

        public bool Send(WishListLetterRequest request)
        {
            try
            {
                _client.Post(request);
                long number = Interlocked.Increment(ref _sentLetters);
                Logger.Debug("Letter \t{2}#{1} sent from {0}", request.Name, number, Environment.CurrentManagedThreadId);
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
    }
}