using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using NLog;
using RabbitMQ.Client;
using SantaHo.Console.Modules;
using SantaHo.Core.ApplicationServices;
using SantaHo.Core.Configuration;
using SantaHo.Core.Infrastructure.Extensions;
using SantaHo.Infrastructure.Modules;
using SantaHo.Infrastructure.Rabbit;
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

        private IConnection _connection;
        private List<IApplicationService> _services = new List<IApplicationService>();

        public bool Start()
        {
            FailIfNot(MigrateSettings);
            FailIfNot(OpenConnections);

            _services = GetServices();
            foreach (IApplicationService service in _services)
            {
                FailIfNot(() => service.Start());
            }

            return true;
        }

        public void Stop()
        {
            Logger.Info("Application is stopping...");

            this.IgnoreFailureWhen(x => x.CloseConnections());
            foreach (IApplicationService service in _services)
            {
                service.IgnoreFailureWhen(x => x.Stop());
            }
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

        private void CloseConnections()
        {
            if (_connection != null)
            {
                _connection.IgnoreFailureWhen(x => x.Close());
            }
        }

        private void OpenConnections()
        {
            _connection = RabbitConnectionFactory.Connect();
            _kernel.Bind<IConnection>().ToConstant(_connection);
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