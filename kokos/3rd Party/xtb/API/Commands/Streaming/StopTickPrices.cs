using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Commands.Streaming
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;
    using JSONArray = Newtonsoft.Json.Linq.JArray;

    public class StopTickPrices : APIStreamingStopCommand
    {
        public StopTickPrices(IEnumerable<string> symbols)
            : base("stopTickPrices")
        {
            JSONArray symbolsArray = new JSONArray();

            foreach (string symbol in symbols)
            {
                symbolsArray.Add(symbol);
            }

            base.AddField("symbols", symbolsArray);
        }
    }
}
