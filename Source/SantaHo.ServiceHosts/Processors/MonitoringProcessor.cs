using Nelibur.ServiceModel.Services.Operations;
using SantaHo.ServiceContracts.Monitoring;

namespace SantaHo.ServiceHosts.Processors
{
    public class MonitoringProcessor : IPostOneWay<HartbeatRequest>
    {
        public void PostOneWay(HartbeatRequest request)
        {
        }
    }
}