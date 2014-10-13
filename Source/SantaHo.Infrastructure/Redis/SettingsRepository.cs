using SantaHo.Domain.Configuration;
using ServiceStack.Text;
using StackExchange.Redis;

namespace SantaHo.Infrastructure.Redis
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly IDatabase _database;

        public SettingsRepository(ConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }

        public TValue Get<TValue>(string key)
        {
            RedisValue value = _database.StringGet(key);
            return new JsonSerializer<TValue>().DeserializeFromString(value);
        }

        public void Set<TValue>(string key, TValue value)
        {
            string serializedValue = new JsonSerializer<TValue>().SerializeToString(value);
            _database.StringSet(key, serializedValue);
        }
    }
}