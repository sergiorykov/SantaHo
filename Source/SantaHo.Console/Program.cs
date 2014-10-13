using Topshelf;

namespace SantaHo.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            HostFactory.New(x =>
            {
                x.Service<ApplicationService>();
                x.RunAsLocalSystem();
                x.StartAutomatically();

                x.SetDescription("Santa Ho Host");
                x.SetDisplayName("SantaHo");
                x.SetServiceName("SantaHo");
            }).Run();
        }
    }
}