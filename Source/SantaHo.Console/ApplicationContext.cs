using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using NLog;
using SantaHo.Core.ApplicationServices;
using SantaHo.Core.ApplicationServices.Resources;
using SantaHo.Core.Configuration;
using SantaHo.Infrastructure.Core.ApplicationServices.Resources;
using SantaHo.Infrastructure.Core.Extensions;
using SantaHo.SantaOffice.Service.Bootstrap;

namespace SantaHo.SantaOffice.Service
{
    public class ApplicationContext
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private List<IApplicationService> _services = new List<IApplicationService>();

        private readonly IKernel _kernel = new StandardKernel(
            new InfrastructureModule(),
            new IncomingLettersModule(),
            new PresentsModule(),
            new SantaOfficeModule());

        private readonly List<IDisposable> _resources = new List<IDisposable>();
        private readonly IStartupSettings _startupSettings = new AppStartupSettings();

        public bool Start()
        {
            FailIfNot(LoadResources);
            FailIfNot(MigrateSettings);

            _services = GetServices();
            foreach (IApplicationService service in _services)
            {
                FailIfNot(() => service.Start(null));
            }

            return true;
        }

        private void LoadResources()
        {
            List<IApplicationResource> resources = _kernel.GetAll<IApplicationResource>().ToList();
            resources.ForEach(x => x.FailIfNot(resource => resource.Load(_startupSettings)));
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
