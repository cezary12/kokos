using System.Collections.Generic;

namespace xAPI.Commands.ReqRes
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;
    using JSONArray = Newtonsoft.Json.Linq.JArray;

    public class GetTickPricesCommand : APICommand
    {
        public GetTickPricesCommand(IEnumerable<string> symbols, int level)
            : base("getTickPrices")
        {
            base.AddField("level", level);

            JSONArray symbolsArray = new JSONArray();

            foreach (string symbol in symbols)
            {
                symbolsArray.Add(symbol);
            }

            base.AddField("symbols", symbolsArray);
        }
    }
}