using StackExchange.Redis;

namespace SantaHo.Infrastructure.Redis
{
    public static class RedisConnectionFactory
    {
        public static ConnectionMultiplexer Create()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("192.168.242.129:6379");
            return redis;
        }
    }
}