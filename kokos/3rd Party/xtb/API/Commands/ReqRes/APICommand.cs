using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Commands.ReqRes
{
    using Newtonsoft.Json.Linq;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public abstract class APICommand
    {
        public string Name { get; private set; }
        public string CustomTag { get; set; }
        private Dictionary<string, JToken> Parameters { get; set; }

        public APICommand(string name)
        {
            this.Name = name;
            this.Parameters = new Dictionary<string, JToken>();
        }

        public void AddField(string name, JToken value)
        {
            Parameters.Add(name, value);
        }

        public JSONObject JSON 
        {
            get
            {
                JSONObject command = new JSONObject();
                command.Add("command", Name);

                JSONObject arguments = new JSONObject();
                foreach (KeyValuePair<string, JToken> argument in Parameters)
                {
                    arguments.Add(argument.Key, argument.Value);
                }
                command.Add("arguments", arguments);

                if (CustomTag != null)
                {
                    command.Add("customTag", CustomTag);
                }

                return command;
            }
        }

        public string JSONString
        {
            get
            {
                return JSON.ToString();
            }
        }
            
    }
}
