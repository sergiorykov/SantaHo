using System;

namespace SantaHo.Core
{
    public class DisposableAction : IDisposable
    {
        private Action _action;

        private DisposableAction(Action action)
        {
            _action = action;
        }

        public static IDisposable From(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            return new DisposableAction(action);
        }
        
        public void Dispose()
        {
            _action();
        }
    }
}