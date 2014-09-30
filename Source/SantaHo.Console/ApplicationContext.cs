using System;
using System.Collections.Generic;
using Ninject;
using NLog;
using RabbitMQ.Client;
using SantaHo.Console.Dependencies;
using SantaHo.Core;
using SantaHo.Core.ApplicationServices;
using SantaHo.Infrastructure.Queues;

namespace SantaHo.Console
{
    public class ApplicationContext
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IKernel _kernel = new StandardKernel(
            new NinjectSettings(),
            new ApplicationModule());

        private IConnection _connection;

        public IDisposable Start()
        {
            OpenConnections();
            foreach (IApplicationService service in GetServices())
            {
                Start(service);
            }

            return DisposableAction.From(Stop);
        }

        private void Stop()
        {
            CloseConnections();
            foreach (IApplicationService service in GetServices())
            {
                ExecuteIgnoreResult(() => service.Stop());
            }
        }

        private void Start(IApplicationService service)
        {
            try
            {
                service.Start();
            }
            catch (Exception e)
            {
                Logger.Warn(e);
                Logger.Error("Application failed to start. Stopping...");
                Stop();
            }
        }

        private void CloseConnections()
        {
            if (_connection != null)
            {
                ExecuteIgnoreResult(() => _connection.Close());
            }
        }

        private static void ExecuteIgnoreResult(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                Logger.Warn(e);
            }
        }

        private void OpenConnections()
        {
            _connection = RabbitConnectionFactory.Connect();
            _kernel.Bind<IConnection>().ToConstant(_connection);
        }

        private IEnumerable<IApplicationService> GetServices()
        {
            return _kernel.GetAll<IApplicationService>();
        }
    }
}