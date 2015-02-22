using Topshelf;

namespace SantaHo.SantaOffice.Service
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            HostFactory.New(x =>
            {
                x.Service<ApplicationContext>(s =>
                {
                    s.ConstructUsing(() => new ApplicationContext());
                    s.WhenStarted(app => app.Start());
                    s.WhenStopped(app => app.Stop());
                });
                x.RunAsLocalSystem();
                x.StartAutomatically();

                x.SetDescription("Santa Ho Host");
                x.SetDisplayName("Santa Ho");
                x.SetServiceName("SantaHo");
            }).Run();
        }
    }
}