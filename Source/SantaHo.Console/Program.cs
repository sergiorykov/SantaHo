namespace SantaHo.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var application = new ApplicationContext();
            using (application.Start())
            {
                System.Console.ReadKey();
            }
        }
    }
}