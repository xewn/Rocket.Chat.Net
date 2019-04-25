using System;

namespace Rocket.Chat.Net.Portability.Websockets
{

    public class PortableErrorEventArgs
    {
        public PortableErrorEventArgs(Exception exception)
        {
            Exception = exception;
        }

        public PortableErrorEventArgs()
        {
        }

        public Exception Exception { get; set; }
    }
}