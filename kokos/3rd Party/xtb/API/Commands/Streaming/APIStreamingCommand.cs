using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Commands.Streaming
{
    using Newtonsoft.Json.Linq;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public abstract class APIStreamingCommand
    {
        public  string Name { get; private set; }
        public abstract JSONObject JSON { get; }
        public abstract string JSONString { get; }

        protected Dictionary<string, JToken> Parameters { get; set; }

        public APIStreamingCommand(string name)
        {
            this.Name = name;
            this.Parameters = new Dictionary<string, JToken>();
        }

        public void AddField(string name, JToken value)
        {
            Parameters.Add(name, value);
        }
    }
}
