using RabbitMQ.Client;
using SantaHo.Domain.IncomingLetters;
using SantaHo.Infrastructure.Factories;
using SantaHo.Infrastructure.Queues;

namespace SantaHo.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IConnection connection = RabbitConnectionFactory.Connect();

            var incomingQueue = new IncomingLettersQueue(connection);
            incomingQueue.Create();
            incomingQueue.Enque(new Letter
            {
                Name = "Vasia",
                Wishes =
                {
                    "Lego",
                    "Transformers"
                }
            });
            connection.Close();

            var application = new ApplicationContext();
            using (application.Start())
            {
                System.Console.ReadKey();
            }
        }
    }
}