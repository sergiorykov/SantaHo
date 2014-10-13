namespace SantaHo.Core.Processing
{
    public interface ITaskFactory<in TValue, out TTask> where TTask : SantaTask<TValue>
    {
        TTask Create(TValue value);
    }
}