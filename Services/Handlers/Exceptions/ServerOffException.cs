using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Handlers.Exceptions
{
    [Serializable]
    public class ServerOffException : Exception
    {
        public ServerOffException() { }

        public ServerOffException(string message) : base(message) { }
    }
}
