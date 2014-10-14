namespace SantaHo.Core.Processing
{
    public abstract class ProcessingTask
    {
        public void Execute()
        {
            ExecuteCore();
        }

        protected abstract void ExecuteCore();
    }

    public abstract class ProcessingTask<TValue> : ProcessingTask
    {
        protected ProcessingTask(TValue value)
        {
            Value = value;
        }

        protected TValue Value { get; private set; }
    }
}