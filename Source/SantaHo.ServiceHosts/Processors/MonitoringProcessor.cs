using Nelibur.ServiceModel.Services.Operations;
using SantaHo.ServiceContracts.Monitoring;

namespace SantaHo.ServiceHosts.Processors
{
    public sealed class MonitoringProcessor : IPostOneWay<HartbeatRequest>
    {
        public void PostOneWay(HartbeatRequest request)
        {
        }
    }
}