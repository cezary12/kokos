using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Commands.Streaming
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;
    using JSONArray = Newtonsoft.Json.Linq.JArray;

    public class GetTickPrices : APIStreamingSubscribeCommand
    {
        public GetTickPrices(string streamSessionId, IEnumerable<string> symbols, int? minArrivalTime = null, int? maxLevel = null)
            : base("getTickPrices", streamSessionId)
        {
            JSONArray symbolsArray = new JSONArray();

            foreach (string symbol in symbols)
            {
                symbolsArray.Add(symbol);
            }

            base.AddField("symbols", symbolsArray);

            if (minArrivalTime != null)
                this.AddField("minArrivalTime", minArrivalTime);
            if (maxLevel != null)
                this.AddField("maxLevel", maxLevel);    
        }
    }
}
