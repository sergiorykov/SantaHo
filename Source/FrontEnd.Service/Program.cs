using Topshelf;

namespace SantaHo.FrontEnd.Service
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            HostFactory.New(x =>
            {
                x.Service<ApplicationContext>(s =>
                {
                    s.WhenStarted(app => app.Start());
                    s.WhenStopped(app => app.Stop());
                });
                x.RunAsLocalSystem();
                x.StartAutomatically();

                x.SetDescription("SantaHo FrontEnd Service");
                x.SetDisplayName("SantaHo FrontEnd Service");
                x.SetServiceName("SantaHoFrontEnd");
            }).Run();
        }
    }
}