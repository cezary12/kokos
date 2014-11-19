using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Connection
{
    interface IConnector
    {
        void Connect(Server server);
        bool IsConnected();
        void Disconnect();
    }
}
