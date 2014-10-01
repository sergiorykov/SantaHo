﻿using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using NLog;
using RabbitMQ.Client;
using SantaHo.Console.Modules;
using SantaHo.Core;
using SantaHo.Core.ApplicationServices;
using SantaHo.Infrastructure.Factories;
using SantaHo.Infrastructure.Modules;
using SantaHo.ServiceHosts.Modules;

namespace SantaHo.Console
{
    public class ApplicationContext
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IKernel _kernel = new StandardKernel(
            new InfrastructureModule(),
            new ApplicationModule(),
            new ServiceHostsModule());

        private IConnection _connection;
        private List<IApplicationService> _services = new List<IApplicationService>();

        public IDisposable Start()
        {
            FailIfNot(OpenConnections);

            _services = GetServices();
            foreach (IApplicationService service in _services)
            {
                FailIfNot(() => service.Start());
            }

            return DisposableAction.From(Stop);
        }

        private void Stop()
        {
            ExecuteIgnoreResult(CloseConnections);
            foreach (IApplicationService service in _services)
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

        private List<IApplicationService> GetServices()
        {
            return _kernel.GetAll<IApplicationService>().ToList();
        }
    }
}