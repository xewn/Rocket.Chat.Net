using System;
using System.Threading.Tasks;
using Rocket.Chat.Net.Bot;
using Rocket.Chat.Net.Bot.Interfaces;
using Rocket.Chat.Net.Interfaces;
using Rocket.Chat.Net.Loggers;
using Rocket.Chat.Net.Models.LoginOptions;

namespace Rocket.Chat.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MainAsync();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }
        private static async Task MainAsync()
        {
            const string username = "system";
            const string password = "abc123!@#";
            const string rocketServerUrl = "192.168.31.157:8818"; // just the host and port
            const bool useSsl = false; // Basically use ws or wss.
            try
            {
                // Create the bot - an abstraction of the driver
                RocketChatBot bot = new RocketChatBot(rocketServerUrl, useSsl);

                // Connect to Rocket.Chat
                await bot.ConnectAsync();

                // Login
                ILoginOption loginOption = new EmailLoginOption
                {
                    Email = username,
                    Password = password
                };
                await bot.LoginAsync(loginOption);

                // Start listening for messages
                await bot.SubscribeAsync();
            }
            catch (Exception ex)
            {
                throw;
            }


        }
    }
}
