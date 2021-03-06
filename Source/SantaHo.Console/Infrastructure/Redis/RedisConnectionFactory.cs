﻿using System;
using System.Configuration;
using SantaHo.Core.ApplicationServices;
using SantaHo.Core.ApplicationServices.Resources;
using SantaHo.Infrastructure.Core.Extensions;
using StackExchange.Redis;

namespace SantaHo.SantaOffice.Service.Infrastructure.Redis
{
    public sealed class RedisConnectionFactory : IApplicationResource
    {
        public const string UriStartupKey = "Redis:Uri";
        private ConnectionMultiplexer _redis;

        public void Dispose()
        {
            if (_redis != null)
            {
                _redis.IgnoreFailureWhen(x => x.Close());
                _redis = null;
            }
        }

        public void Load(IStartupSettings settings)
        {
            var connectionUri = settings.GetValue<string>(UriStartupKey);
            _redis = ConnectionMultiplexer.Connect(connectionUri);
        }

        public IDatabase GetDatabase()
        {
            if (_redis == null)
            {
                throw new ConfigurationErrorsException("Redis connection must be loaded");
            }

            return _redis.GetDatabase();
        }
    }
}
