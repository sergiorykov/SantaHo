using Nelibur.ServiceModel.Services.Operations;
using SantaHo.FrontEnd.ServiceContracts.Monitoring;

namespace SantaHo.FrontEnd.Service.Hosts.Handlers
{
    public sealed class HartbeatRequestHandler : IPostOneWay<HartbeatRequest>
    {
        public void PostOneWay(HartbeatRequest request)
        {
        }
    }
}