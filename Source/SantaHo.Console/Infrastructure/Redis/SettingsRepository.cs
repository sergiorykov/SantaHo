using Newtonsoft.Json;
using SantaHo.Core.Configuration;
using StackExchange.Redis;

namespace SantaHo.Infrastructure.Redis
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly IDatabase _database;
        private readonly KeyEvaluator _keyEvaluator;

        public SettingsRepository(RedisConnectionFactory connectionFactory, KeyEvaluator keyEvaluator)
        {
            _keyEvaluator = keyEvaluator;
            _database = connectionFactory.GetDatabase();
        }

        public TValue Get<TValue>() where TValue : class
        {
            string key = _keyEvaluator.GetKey<TValue>();
            RedisValue value = _database.StringGet(key);
            return JsonConvert.DeserializeObject<TValue>(value);
        }

        public void Set<TValue>(TValue value) where TValue : class
        {
            string key = _keyEvaluator.GetKey<TValue>();
            string serializedValue = JsonConvert.SerializeObject(value);
            _database.StringSet(key, serializedValue);
        }
    }
}