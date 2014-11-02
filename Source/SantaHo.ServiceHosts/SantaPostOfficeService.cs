using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using Nelibur.ServiceModel.Services;
using Nelibur.ServiceModel.Services.Default;
using Nelibur.ServiceModel.Services.Operations;
using Ninject;
using SantaHo.Core.Infrastructure;
using SantaHo.Core.Infrastructure.ApplicationServices;
using SantaHo.ServiceContracts.Letters;
using SantaHo.ServiceContracts.Monitoring;
using SantaHo.ServiceHosts.Mappings;
using SantaHo.ServiceHosts.Modules;
using SantaHo.ServiceHosts.Processors;

namespace SantaHo.ServiceHosts
{
    public class SantaPostOfficeService : NinjectApplicationService
    {
        private ServiceHost _serviceHost;

        public SantaPostOfficeService(IKernel kernel) : base(kernel)
        {
            kernel.Load(new ServiceHostsModule());

            NeliburRestService.Configure(x =>
            {
                Bind<WishListLetterRequest, WishListProcessor>(x);
                Bind<HartbeatRequest, MonitoringProcessor>(x);
            });

            ContractsMappingManager.Configure();
        }

        private void Bind<TRequest, TProcessor>(IConfiguration configuration) where TRequest : class
            where TProcessor : IRequestOperation
        {
            Func<TProcessor> processorFactory = () => Kernel.Get<TProcessor>();
            configuration.Bind<TRequest, TProcessor>(processorFactory);
        }

        public override void Start()
        {
            _serviceHost = new WebServiceHost(typeof (JsonServicePerCall));
            _serviceHost.Open();
        }

        public override void Stop()
        {
            if (_serviceHost != null)
            {
                _serviceHost.Close();
            }
        }
    }
}