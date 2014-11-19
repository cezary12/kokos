using System.Collections.Generic;
using System;

namespace xAPI.Records
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;
    using JSONArray = Newtonsoft.Json.Linq.JArray;

    public class ProfitRecord : APIRecord
    {
        /// <summary>
        /// Order number.
        /// </summary>
        public long Order { get; private set; }

        /// <summary>
        /// Transaction ID.
        /// </summary>
        public long Order2 { get; private set; }

        /// <summary>
        /// Position number.
        /// </summary>
        public long Position { get; private set; }

        /// <summary>
        /// Profit in account currency.
        /// </summary>
        public double Profit { get; private set; }

        /// <summary>
        /// Price of the symbol that has triggered the computation of this profit.
        /// </summary>
        public double ClosePrice { get; private set; }
        
        public ProfitRecord(JSONObject body) 
        {
            this.Order = (long)body["order"];
            this.Order2 = (long)body["order2"];
            this.Position = (long)body["position"];
            this.Profit = (double)body["profit"];
            this.ClosePrice = (double)body["closePrice"];
        }
    }
}
