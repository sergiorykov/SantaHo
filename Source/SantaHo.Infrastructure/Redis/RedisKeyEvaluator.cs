using SantaHo.Core.Configuration;
using SantaHo.Core.Extensions;

namespace SantaHo.Infrastructure.Redis
{
    public class RedisKeyEvaluator : KeyEvaluator
    {
        public RedisKeyEvaluator()
        {
            KeyPrefix = "settings:{0}".FormatWith(KeyPrefix);
        }

        public RedisKeyEvaluator(string groupName) : base(groupName)
        {
            KeyPrefix = "settings:{0}".FormatWith(KeyPrefix);
        }
    }
}