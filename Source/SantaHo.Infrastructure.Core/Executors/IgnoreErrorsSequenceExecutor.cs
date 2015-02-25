using System;
using System.Collections.Generic;
using SantaHo.Infrastructure.Core.Extensions;

namespace SantaHo.Infrastructure.Core.Executors
{
    public class IgnoreErrorsSequenceExecutor<TTarget> : SequenceExecutor<TTarget>
    {
        public IgnoreErrorsSequenceExecutor(IEnumerable<TTarget> values) : base(values)
        {
        }

        protected override void ExecuteCore(Action<TTarget> action)
        {
            Values.ForEach(x => x.IgnoreFailureWhen(action));
        }
    }
}
