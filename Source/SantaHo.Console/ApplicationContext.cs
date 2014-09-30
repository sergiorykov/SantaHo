using System;
using System.Collections.Generic;
using Ninject;
using NLog;
using RabbitMQ.Client;
using SantaHo.Console.Modules;
using SantaHo.Core;
using SantaHo.Core.ApplicationServices;
using SantaHo.Infrastructure.Factories;

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
            FailIfNot(OpenConnections);
            foreach (IApplicationService service in GetServices())
            {
                FailIfNot(() => service.Start());
            }

            return DisposableAction.From(Stop);
        }

        private void Stop()
        {
            ExecuteIgnoreResult(CloseConnections);
            foreach (IApplicationService service in GetServices())
            {
                ExecuteIgnoreResult(() => service.Stop());
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

                Logger.Error("Application failed to start. Stopping...");
                Stop();

                throw new InvalidOperationException("Application failed to start");
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