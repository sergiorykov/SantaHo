using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using SantaHo.Infrastructure.Core.Extensions;

namespace SantaHo.Infrastructure.Core.Executors
{
    public class RallbackOnErrorSequenceExecutor<TTarget> : SequenceExecutor<TTarget>
    {
        private readonly Action<TTarget> _rallback;

        public RallbackOnErrorSequenceExecutor(IEnumerable<TTarget> values, Action<TTarget> rallback) : base(values)
        {
            if (rallback == null)
            {
                throw new ArgumentNullException("rallback");
            }

            _rallback = rallback;
        }

        protected override void ExecuteCore(Action<TTarget> action)
        {
            List<TTarget> succeededValues = Values.TakeWhile(x => x.FailIfNot(action)).ToList();
            if (succeededValues.Count == Values.Count)
            {
                return;
            }

            succeededValues.ForEach(x => x.IgnoreFailureWhen(_rallback));
            throw new InvalidOperationException("All operations cancelled");
        }
    }
}