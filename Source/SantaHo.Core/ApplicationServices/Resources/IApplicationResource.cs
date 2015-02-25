using System;

namespace SantaHo.Core.ApplicationServices.Resources
{
    public interface IApplicationResource : IDisposable
    {
        void Load(IStartupSettings startupSettings);
    }
}
