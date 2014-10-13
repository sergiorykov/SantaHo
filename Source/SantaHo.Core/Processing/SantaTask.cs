namespace SantaHo.Core.Processing
{
    public abstract class SantaTask
    {
        public void Execute()
        {
            ExecuteCore();
        }

        protected abstract void ExecuteCore();
    }

    public abstract class SantaTask<TValue> : SantaTask
    {
        protected SantaTask(TValue value)
        {
            Value = value;
        }

        public TValue Value { get; private set; }
    }
}