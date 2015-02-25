using System;

namespace SantaHo.Infrastructure.Core.Executors
{
    public interface IExecutor<out TTarget>
    {
        void Execute(Action<TTarget> action);
    }


    public interface IExecutor
    {
        void Execute(Action action);
        TResult Execute<TResult>(Func<TResult> action);
    }
}
