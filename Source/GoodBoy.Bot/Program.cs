using System;
using FluentScheduler;
using GoodBoy.Bot.Modules;
using GoodBoy.Bot.Schedulers;
using Ninject;

namespace GoodBoy.Bot
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var kernel = new StandardKernel(new GoodBoyModule());
            TaskManager.TaskFactory = new GoodBoyTaskFactory(kernel);
            TaskManager.Initialize(new GoodBoyRegistry());

            Console.ReadKey();
        }
    }
}