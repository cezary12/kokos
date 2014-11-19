using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Commands.Streaming
{
    using Newtonsoft.Json.Linq;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public abstract class APIStreamingSubscribeCommand : APIStreamingCommand
    {

        public string StreamSessionId { get; private set; }

        public APIStreamingSubscribeCommand(string commandName, string streamSessionId)
            : base(commandName)
        {
            this.StreamSessionId = streamSessionId;
        }

        public override JSONObject JSON 
        {
            get
            {
                JSONObject command = new JSONObject();
                command.Add("command", Name);
                command.Add("streamSessionId", StreamSessionId);
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
