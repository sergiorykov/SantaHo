using SantaHo.Core.Configuration;

namespace SantaHo.Infrastructure.Redis
{
    public class RedisKeyEvaluator : KeyEvaluator
    {
        public RedisKeyEvaluator()
        {
        }

        public RedisKeyEvaluator(string groupName) : base(groupName)
        {
        }
    }
}