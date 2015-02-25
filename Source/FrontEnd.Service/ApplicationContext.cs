using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using NLog;
using SantaHo.Core.ApplicationServices;
using SantaHo.Core.ApplicationServices.Resources;
using SantaHo.FrontEnd.Service.Bootstrap;
using SantaHo.Infrastructure.Core.ApplicationServices.Resources;
using SantaHo.Infrastructure.Core.Executors;

namespace SantaHo.FrontEnd.Service
{
    public sealed class ApplicationContext
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private List<IApplicationResource> _resources = new List<IApplicationResource>();
        private List<IApplicationService> _services = new List<IApplicationService>();

        private readonly IKernel _kernel = new StandardKernel(
            new HostsModule(),
            new QueuesModule());

        public bool Start()
        {
            Logger.Info("Application is starting...");
            try
            {
                StartCore();
            }
            catch (Exception e)
            {
                Logger.Error("Application failed to start", e);
                return false;
            }

            Logger.Info("Application is started");
            return true;
        }

        private void StartCore()
        {
            IStartupSettings startupSettings = new AppStartupSettings();

            _resources = GetResources();
            SequenceExecutor.For(_resources)
                            .RallbackOnError(x => x.Dispose())
                            .Execute(x => x.Load(startupSettings));

            _services = GetServices();
            SequenceExecutor.For(_services)
                            .RallbackOnError(x => x.Stop())
                            .Execute(x => x.Start(startupSettings));
        }

        public void Stop()
        {
            Logger.Info("Application is stopping...");
            SequenceExecutor.For(_services)
                            .IgnoreErrors()
                            .Execute(x => x.Stop());

            SequenceExecutor.For(_resources)
                            .IgnoreErrors()
                            .Execute(x => x.Dispose());

            Logger.Info("Application is stopped");
        }

        private List<IApplicationService> GetServices()
        {
            return _kernel.GetAll<IApplicationService>().ToList();
        }

        private List<IApplicationResource> GetResources()
        {
            return _kernel.GetAll<IApplicationResource>().ToList();
        }
    }
}
