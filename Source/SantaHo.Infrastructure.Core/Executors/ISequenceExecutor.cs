namespace SantaHo.Infrastructure.Core.Executors
{
    public interface ISequenceExecutor<out TExecutor, out TTarget> : IExecutor<TTarget>
        where TExecutor : ISequenceExecutor<TExecutor, TTarget>
    {
    }
}