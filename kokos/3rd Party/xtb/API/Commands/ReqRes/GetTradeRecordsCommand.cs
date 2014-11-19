using System.Collections.Generic;

namespace xAPI.Commands.ReqRes
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;
    using JSONArray = Newtonsoft.Json.Linq.JArray;

    public class GetTradeRecordsCommand : APICommand
    {
        public GetTradeRecordsCommand(IEnumerable<int> orders)
            : base("getTradeRecords")
        {
            JSONArray ordersArray = new JSONArray();

            foreach (int order in orders)
            {
                ordersArray.Add(order);
            }

            base.AddField("orders", ordersArray);
        }
    }

}