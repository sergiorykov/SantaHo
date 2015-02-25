using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using Nelibur.ServiceModel.Services;
using Nelibur.ServiceModel.Services.Default;
using Nelibur.ServiceModel.Services.Operations;
using Ninject;
using NLog;
using SantaHo.Core.ApplicationServices;
using SantaHo.FrontEnd.Service.Hosts.Handlers;
using SantaHo.FrontEnd.ServiceContracts.Letters;
using SantaHo.FrontEnd.ServiceContracts.Monitoring;
using SantaHo.Infrastructure.Core.ApplicationServices;

namespace SantaHo.FrontEnd.Service.Hosts
{
    public sealed class HostApplicationService : NinjectApplicationService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private ServiceHost _serviceHost;

        public HostApplicationService(IKernel kernel)
            : base(kernel)
        {
            ConfigureServieHost();
            ContractsMappings.Configure();
        }

        private void ConfigureServieHost()
        {
            NeliburRestService.Configure(x =>
            {
                Bind<WishListLetterRequest, WishListLetterRequestHandler>(x);
                Bind<HartbeatRequest, HartbeatRequestHandler>(x);
            });
        }

        private void Bind<TRequest, TProcessor>(IConfiguration configuration)
            where TRequest : class
            where TProcessor : IRequestOperation
        {
            Func<TProcessor> processorFactory = () => Kernel.Get<TProcessor>();
            configuration.Bind<TRequest, TProcessor>(processorFactory);
        }

        public override void Start(IStartupSettings startupSettings)
        {
            var hostSettings = HostSettings.Create(startupSettings);
            OpenServiceHost(hostSettings);
        }

        private void OpenServiceHost(HostSettings hostSettings)
        {
            Logger.Info("Opening service host...");
            _serviceHost = new WebServiceHost(typeof(JsonServicePerCall), hostSettings.ServiceHostUri);
            _serviceHost.Open();
            Logger.Info("Opened service host on {0}", hostSettings.ServiceHostUri);
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
