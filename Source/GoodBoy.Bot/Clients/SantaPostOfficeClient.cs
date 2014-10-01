using System;
using System.Threading.Tasks;
using GoodBoy.Bot.Properties;
using Nelibur.ServiceModel.Clients;
using NLog;
using SantaHo.ServiceContracts.Letters;

namespace GoodBoy.Bot.Clients
{
    public sealed class SantaPostOfficeClient : ISantaPostOfficeClient
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly JsonServiceClient _client;

        public SantaPostOfficeClient()
        {
            string address = Settings.Default.SantaPostOfficeAddress;
            _client = new JsonServiceClient(address);
        }

        public async Task<bool> Send(WishListLetterRequest request)
        {
            try
            {
                await _client.PostAsync(request);
                Logger.Debug("Letter sent from {0}", request.Name);

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