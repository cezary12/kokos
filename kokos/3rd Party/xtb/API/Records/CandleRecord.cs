using System;

namespace xAPI.Records
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class CandleRecord : APIRecord
    {
        /// <summary>
        /// Close price in base currency.
        /// </summary>
        public double Close { get; set; }

        /// <summary>
        /// Highest value in the given period in base currency.
        /// </summary>
        public double High { get; set; }

        /// <summary>
        /// Lowest value in the given period in base currency.
        /// </summary>
        public double Low { get; set; }

        /// <summary>
        /// Open price in base currency.
        /// </summary>
        public double Open { get; set; }

        /// <summary>
        /// Candle start time in CET time zone (Central European Time).
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// string representation of the timestamp field.
        /// </summary>
        public string TimeString { get; set; }

        public CandleRecord(JSONObject body)
        {
            this.Close = (double)body["close"];
            this.High = (double)body["high"];
            this.Low = (double)body["low"];
            this.Open = (double)body["open"];            
            this.Timestamp = (long)body["timestamp"];
            this.TimeString = (string)body["timeString"];
        }
    }
}