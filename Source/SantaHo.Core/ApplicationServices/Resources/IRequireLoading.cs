using System;

namespace SantaHo.Core.ApplicationServices.Resources
{
    public interface IRequireLoading : IDisposable
    {
        void Load(IStartupSettings settings);
    }
}
