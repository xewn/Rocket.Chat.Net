﻿namespace Rocket.Chat.Net.Example
{
    using System;
    using System.Threading.Tasks;

    using Rocket.Chat.Net.Driver;
    using Rocket.Chat.Net.Interfaces;
    using Rocket.Chat.Net.Models.LoginOptions;

    public class DriverExample
    {
        public async Task Run()
        {
            const string username = "a@qq.com";
            const string password = "abc123!@#";
            const string rocketServerUrl = "192.168.31.137:8818"; // just the host and port
            const bool useSsl = false;

            ILoginOption loginOption = new LdapLoginOption
            {
                Username = username,
                Password = password
            };

            IRocketChatDriver driver = new RocketChatDriver(rocketServerUrl, useSsl);

            await driver.ConnectAsync(); // Connect to server
            await driver.LoginAsync(loginOption); // Login via LDAP
            await driver.SubscribeToRoomAsync(); // Listen on all rooms

            driver.MessageReceived += MesReceived;

            var generalRoomIdResult = await driver.GetRoomIdAsync("general");
            if (generalRoomIdResult.HasError)
            {
                throw new Exception("Could not find room by name.");
            }

            var generalRoomId = generalRoomIdResult.Result;
            await driver.SendMessageAsync("message", generalRoomId);
        }
        private void MesReceived(object info)
        {
            Console.WriteLine(info);
        }
    }
}