namespace Rocket.Chat.Net.Example
{
    using System;
    using System.Threading.Tasks;

    using Rocket.Chat.Net.Bot;
    using Rocket.Chat.Net.Bot.Interfaces;
    using Rocket.Chat.Net.Interfaces;
    using Rocket.Chat.Net.Loggers;
    using Rocket.Chat.Net.Models.LoginOptions;

    public static class Program
    {
        public static void Main()
        {
            DriverExample driverExample = new DriverExample();
            Task.Run(async () => await driverExample.Run());
            Console.ReadLine();
        }

    }
}
