using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Commands.Streaming
{
    using Newtonsoft.Json.Linq;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public abstract class APIStreamingStopCommand : APIStreamingCommand
    {

        public APIStreamingStopCommand(string commandName)
            : base(commandName)
        {
        }

        public override JSONObject JSON
        {
            get
            {
                JSONObject command = new JSONObject();
                command.Add("command", Name);
                foreach (KeyValuePair<string, JToken> argument in Parameters)
                {
                    command.Add(argument.Key, argument.Value);
                }
                return command;
            }
        }

        public override string JSONString
        {
            get
            {
                return JSON.ToString();
            }
        }
    }
}
