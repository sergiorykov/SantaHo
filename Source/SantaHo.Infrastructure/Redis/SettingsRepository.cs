using SantaHo.Domain.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace SantaHo.Infrastructure.Redis
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly IDatabase _database;
        private readonly KeyEvaluator _keyEvaluator;

        public SettingsRepository(ConnectionMultiplexer connection, KeyEvaluator keyEvaluator)
        {
            _keyEvaluator = keyEvaluator;
            _database = connection.GetDatabase();
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