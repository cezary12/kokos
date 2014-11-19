using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xAPI.Records
{
    using xAPI.Codes;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class IbRecord : APIRecord
    {
        /// <summary>
        /// IB close price or null if not allowed to view.
        /// </summary>
        public double? ClosePrice { get; set; }

        /// <summary>
        /// IB user login or null if not allowed to view.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// IB nominal or null if not allowed to view.
        /// </summary>
        public double? Nominal { get; set; }

        /// <summary>
        /// IB open price or null if not allowed to view.
        /// </summary>
        public double? OpenPrice { get; set; }

        /// <summary>
        /// Operation code or null if not allowed to view.
        /// </summary>
        public Side Side { get; set; }

        /// <summary>
        /// IB user surname or null if not allowed to view.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Symbol or null if not allowed to view.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Time the record was created or null if not allowed to view.
        /// </summary>
        public long? Timestamp { get; set; }

        /// <summary>
        /// Volume in lots or null if not allowed to view.
        /// </summary>
        public double? Volume { get; set; }

        public IbRecord(JSONObject body)
        {
            this.ClosePrice = (double?)body["closePrice"];
            this.Login = (string)body["login"];
            this.Nominal = (double?)body["nominal"];
            this.OpenPrice = (double?)body["openPrice"];
            this.Side = Side.FromCode((int)body["side"]);
            this.Surname = (string)body["surname"];
            this.Symbol = (string)body["symbol"];
            this.Timestamp = (long?)body["timestamp"];
            this.Volume = (double?)body["volume"];
        }
    }
}
