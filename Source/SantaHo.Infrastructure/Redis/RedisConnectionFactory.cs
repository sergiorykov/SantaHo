using System.Configuration;
using SantaHo.Core.ApplicationServices.Resources;
using SantaHo.Infrastructure.Core.Extensions;
using StackExchange.Redis;

namespace SantaHo.Infrastructure.Redis
{
    public sealed class RedisConnectionFactory : IRequireLoading
    {
        private ConnectionMultiplexer _redis;

        public void Dispose()
        {
            if (_redis != null)
            {
                _redis.IgnoreFailureWhen(x => x.Close());
                _redis = null;
            }
        }

        public void Load()
        {
            _redis = ConnectionMultiplexer.Connect("192.168.242.129:6379");
        }

        public IDatabase GetSettingsDatabase()
        {
            if (_redis == null)
            {
                throw new ConfigurationErrorsException("Redis connection must be loaded");
            }

            return _redis.GetDatabase();
        }
    }
}