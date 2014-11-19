using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Records
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class IndicatorsRecord : APIRecord
    {
        /// <summary>
        /// Balance in account currency.
        /// </summary>
        public double Balance { get; private set; }

        /// <summary>
        /// Credit in account currency.
        /// </summary>
        public double Credit { get; private set; }

        /// <summary>
        /// Sum of balance and all profits in account currency.
        /// </summary>
        public double Equity { get; private set; }

        /// <summary>
        /// Margin requirements.
        /// </summary>
        public double Margin { get; private set; }

        /// <summary>
        /// Free margin.
        /// </summary>
        public double MarginFree { get; private set; }

        /// <summary>
        /// Margin level percentage.
        /// </summary>
        public double MarginLevel { get; private set; }

        public IndicatorsRecord(JSONObject body)
        {
            this.Balance = (double)body["balance"];
            this.Credit = (double)body["credit"];
            this.Equity = (double)body["equity"];
            this.Margin = (double)body["margin"];
            this.MarginFree = (double)body["marginFree"];
            this.MarginLevel = (double)body["marginLevel"];
        }
    }
}

