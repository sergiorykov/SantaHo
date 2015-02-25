using System;
using System.Collections.Generic;
using System.Linq;

namespace SantaHo.Infrastructure.Core.Executors
{
    public class SequenceExecutor<TTarget> : ISequenceExecutor<SequenceExecutor<TTarget>, TTarget>
    {
        public SequenceExecutor(IEnumerable<TTarget> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            Values = values.ToList();
        }

        protected List<TTarget> Values { get; private set; }

        public void Execute(Action<TTarget> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            ExecuteCore(action);
        }

        protected virtual void ExecuteCore(Action<TTarget> action)
        {
            Values.ForEach(action);
        }

        public SequenceExecutor<TTarget> IgnoreErrors()
        {
            return new IgnoreErrorsSequenceExecutor<TTarget>(Values);
        }

        public SequenceExecutor<TTarget> RallbackOnError(Action<TTarget> rallback)
        {
            return new RallbackOnErrorSequenceExecutor<TTarget>(Values, rallback);
        }
    }
}
