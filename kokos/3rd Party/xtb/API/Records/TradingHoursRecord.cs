using System.Collections.Generic;
using System;

namespace xAPI.Records
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;
    using JSONArray = Newtonsoft.Json.Linq.JArray;

    public class TradingHoursRecord : APIRecord
    {

        /// <summary>
        /// Quotes hours records.
        /// </summary>
        public LinkedList<HoursRecord> Quotes { get; private set; }

        /// <summary>
        /// Symbol.
        /// </summary>
        public string Symbol { get; private set; }

        /// <summary>
        /// Trading hours records.
        /// </summary>
        public LinkedList<HoursRecord> Trading { get; private set; }

        public TradingHoursRecord(JSONObject body)
        {

            this.Quotes = new LinkedList<HoursRecord>();
            this.Trading = new LinkedList<HoursRecord>();

            this.Symbol = (string)body["symbol"];

            if (body["quotes"] != null)
            {
                JSONArray jsonarray = (JSONArray)body["quotes"];
                foreach (JSONObject i in jsonarray)
                {
                    HoursRecord rec = new HoursRecord(i);
                    this.Quotes.AddLast(rec);
                }
            }

            if (body["trading"] != null)
            {
                JSONArray jsonarray = (JSONArray)body["trading"];
                foreach (JSONObject i in jsonarray)
                {
                    HoursRecord rec = new HoursRecord(i);
                    this.Trading.AddLast(rec);
                }
            }
        }
    }
}
