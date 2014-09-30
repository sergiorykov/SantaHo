using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using Nelibur.ServiceModel.Services;
using Nelibur.ServiceModel.Services.Default;
using Nelibur.ServiceModel.Services.Operations;
using Ninject;
using SantaHo.Core.Ninject;
using SantaHo.ServiceContracts.Letters;
using SantaHo.ServiceHosts.Mappings;
using SantaHo.ServiceHosts.Processors;

namespace SantaHo.ServiceHosts
{
    public class SantaPostOfficeService : NinjectApplicationService
    {
        private ServiceHost _serviceHost;

        public SantaPostOfficeService(IKernel kernel) : base(kernel)
        {
            NeliburRestService.Configure(
                Bind<WishListLetterRequest, WishListProcessor>);

            ContractsMappingManager.Configure();
        }

        private void Bind<TRequest, TProcessor>(IConfiguration configuration)where TRequest : class where TProcessor : IRequestOperation
        {
            Func<TProcessor> processorFactory = () => Kirnel.Get<TProcessor>();
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