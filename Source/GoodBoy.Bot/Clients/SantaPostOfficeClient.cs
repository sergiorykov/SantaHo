using System;
using GoodBoy.Bot.Properties;
using Nelibur.ServiceModel.Clients;
using NLog;
using SantaHo.ServiceContracts.Letters;
using ServiceStack.Text;

namespace GoodBoy.Bot.Clients
{
    public sealed class SantaPostOfficeClient : ISantaPostOfficeClient
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
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
                Logger.Debug("Letter sent {0}", request.Dump());

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