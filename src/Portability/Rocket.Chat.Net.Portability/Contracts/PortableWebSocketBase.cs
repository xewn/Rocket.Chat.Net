using System;
using Rocket.Chat.Net.Portability.Websockets;

namespace Rocket.Chat.Net.Portability.Contracts
{
    public abstract class PortableWebSocketBase
    {
        public PortableWebSocketBase(string url)
        {
        }

        public abstract event EventHandler<PortableMessageReceivedEventArgs> MessageReceived;
        public abstract event EventHandler Closed;
        public abstract event EventHandler<PortableErrorEventArgs> Error;
        public abstract event EventHandler Opened;
        public abstract void Open();
        public abstract void Close();
        public abstract void Send(string json);
    }
}