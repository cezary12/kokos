using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Connection
{
    interface ISyncConnector
    {
        void Send(string message);
        string Receive();
        string Execute(string message);
    }
}
