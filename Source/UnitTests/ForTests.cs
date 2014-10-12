using System;
using FluentAssertions;
using StackExchange.Redis;
using Xunit;

namespace UnitTests
{
    public class ForTests
    {
        [Fact(Skip = "only to test connection")]
        public void Redis_Connect_Success()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("192.168.242.129:6379");
            IDatabase database = redis.GetDatabase();

            RedisKey key = Guid.NewGuid().ToString();
            RedisValue value = "asdf";
            database.StringSet(key, value);

            RedisValue actualValue = database.StringGet(key);

            actualValue.Should().NotBeNull("already saved");
            actualValue.Should().Match(x => x.ToString() == value.ToString());
        }
    }
}