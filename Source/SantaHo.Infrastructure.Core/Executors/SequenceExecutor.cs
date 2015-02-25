using System;
using System.Collections.Generic;

namespace SantaHo.Infrastructure.Core.Executors
{
    public static class SequenceExecutor
    {
        public static SequenceExecutor<TTarget> For<TTarget>(IEnumerable<TTarget> values)
        {
            return new SequenceExecutor<TTarget>(values);
        }
    }
}
