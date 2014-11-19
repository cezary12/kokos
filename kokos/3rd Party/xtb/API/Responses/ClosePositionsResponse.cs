using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xAPI.Records;

namespace xAPI.Responses
{
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class ClosePositionsResponse : APIResponse
    {
        /// <summary>
        /// Order numbers of all close requests, in the same order.
        /// </summary>
        public LinkedList<long> Orders;

        public ClosePositionsResponse(string body)
            : base(body)
        {
            this.Orders = new LinkedList<long>();

            JSONArray arr = (JSONArray)this.ReturnData["orders"];
            foreach (JSONObject order in arr)
            {
                this.Orders.AddLast((long)order);
            }
        }
    }
}
