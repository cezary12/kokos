using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Connection
{
    /// <summary>
    /// A class that also holds specific address and port information.
    /// </summary>
    public class SpecificServer : Server
    {
        /// <summary>
        /// Address of the server
        /// </summary>
        public string Address { get; set; }
        
        /// <summary>
        /// Port of the server
        /// </summary>
        new public int Port { get; set; }

        /// <summary>
        /// The URI of the server.
        /// </summary>
        new public string URI {
            get
            {
                return "tcp://" + Address + ":" + Port;
            }
        }

        /// <summary>
        /// Api remote endpoint.
        /// </summary>
        /// <param name="address">Server address</param>
        /// <param name="port">Port</param>
        /// <param name="secure">True if ssl connection should be used</param>
        /// <param name="description">Description of the server</param>
        public SpecificServer(string address, int port, bool secure = true, string description = "CUSTOM")
            : base("tcps://" + address + ":" + port, secure, description)
        {
            this.Address = address;
            this.Port = port;
        }
    }
}
