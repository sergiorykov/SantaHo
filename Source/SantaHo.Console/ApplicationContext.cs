using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using NLog;
using SantaHo.Console.Modules;
using SantaHo.Core.ApplicationServices;
using SantaHo.Core.ApplicationServices.Resources;
using SantaHo.Core.Configuration;
using SantaHo.Infrastructure.Core.Extensions;
using SantaHo.Infrastructure.Modules;
using SantaHo.ServiceHosts.Modules;

namespace SantaHo.Console
{
    public class ApplicationContext
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IKernel _kernel = new StandardKernel(
            new InfrastructureModule(),
            new IncomingLettersModule(),
            new PresentsModule(),
            new SantaOfficeModule(),
            new ServiceHostsModule());

        private readonly List<IDisposable> _resources = new List<IDisposable>();
        private List<IApplicationService> _services = new List<IApplicationService>();

        public bool Start()
        {
            FailIfNot(LoadResources);
            FailIfNot(MigrateSettings);

            _services = GetServices();
            foreach (IApplicationService service in _services)
            {
                FailIfNot(() => service.Start());
            }

            return true;
        }

        private void LoadResources()
        {
            List<IRequireLoading> resources = _kernel.GetAll<IRequireLoading>().ToList();
            resources.ForEach(x => x.FailIfNot(resource => resource.Load()));
            _resources.AddRange(resources);
        }

        public void Stop()
        {
            Logger.Info("Application is stopping...");
            _services.IgnoreFailuresWhen(x => x.Stop());
            _resources.IgnoreFailuresWhen(x => x.Dispose());
        }

        private void FailIfNot(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                Logger.Warn(e);
                Logger.Error("Application failed to start");

                Stop();

                throw new InvalidOperationException("Application failed to start");
            }
        }

        private List<IApplicationService> GetServices()
        {
            return _kernel.GetAll<IApplicationService>().ToList();
        }

        private void MigrateSettings()
        {
            var migration = _kernel.Get<SettingsMigration>();
            List<ISupportSettingsMigration> configurators = _kernel.GetAll<ISupportSettingsMigration>().ToList();
            configurators.ForEach(x => x.PrepareSettings(migration));

            migration.Apply();
        }
    }
}