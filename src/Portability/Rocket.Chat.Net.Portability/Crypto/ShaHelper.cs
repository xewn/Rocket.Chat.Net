using System;
using Rocket.Chat.Net.Portability.Contracts;

namespace Rocket.Chat.Net.Portability.Crypto
{

    public class ShaHelper : ShaHelperBase
    {
        public override string Sha256Hash(string value)
        {
            throw new NotImplementedException();
        }
    }
}