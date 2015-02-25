using System;
using SantaHo.Core.Configuration;

namespace SantaHo.SantaOffice.Service.Infrastructure.Redis
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
