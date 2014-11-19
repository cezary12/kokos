using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xAPI.Connection
{
    public class Server
    {
        /// <summary>
        /// The URI of the server.
        /// </summary>
        public string URI { get; set; }
        
        /// <summary>
        /// True if ssl connection should be used.
        /// </summary>
        public bool Secure { get; set; }

        /// <summary>
        /// Description of the server.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Port of the server read from URI.
        /// </summary>
        public int Port 
        {
            get
            {
                var colonPosition = URI.LastIndexOf(":");
                var portString = URI.Substring(colonPosition + 1);
                return Convert.ToInt32(portString);
            }
        }

        public Server(string uri, bool secure = true, string description = "CUSTOM")
        {
            this.URI = uri;
            this.Secure = secure;
            this.Description = description;
        }        

        /// <summary>
        /// Creates new server based on uri.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public Server WithURI(string uri) 
        {
            return new Server(uri, Secure, Description);
        }
    }
}
