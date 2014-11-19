using System.Collections.Generic;

namespace xAPI.Commands.ReqRes
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;
    using JSONArray = Newtonsoft.Json.Linq.JArray;

	public class GetTradingHoursCommand : APICommand
	{
        public GetTradingHoursCommand(IEnumerable<string> symbols)
            : base("getTradingHours")
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