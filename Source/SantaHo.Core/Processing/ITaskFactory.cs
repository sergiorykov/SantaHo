namespace SantaHo.Core.Processing
{
    public interface ITaskFactory<in TValue, out TTask> where TTask : ProcessingTask<TValue>
    {
        TTask Create(TValue value);
    }
}