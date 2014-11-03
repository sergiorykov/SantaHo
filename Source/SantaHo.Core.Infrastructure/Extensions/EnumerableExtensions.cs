using System;
using System.Collections.Generic;
using System.Linq;

namespace SantaHo.Core.Infrastructure.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool ExecuteAllOrRollback<TTarget>(this IEnumerable<TTarget> values, Action<TTarget> action,
                                                         Action<TTarget> rollback)
        {
            List<TTarget> rollbackValues = values
                .TakeWhile(target => !target.FailIfNot(action))
                .ToList();

            rollbackValues.ForEach(x => x.IgnoreFailureWhen(rollback));
            return rollbackValues.Count == 0;
        }
    }
}