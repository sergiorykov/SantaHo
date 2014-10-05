using Topshelf;

namespace SantaHo.Console
{
    public sealed class ApplicationService : ServiceControl
    {
        private ApplicationContext _context;

        public bool Start(HostControl hostControl)
        {
            _context = new ApplicationContext();
            return _context.Start();
        }

        public bool Stop(HostControl hostControl)
        {
            if (_context != null)
            {
                _context.Stop();
            }

            return true;
        }
    }
}