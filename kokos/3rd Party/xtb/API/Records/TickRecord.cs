using System.Collections.Generic;
using System;

namespace xAPI.Records
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;
    using JSONArray = Newtonsoft.Json.Linq.JArray;   

	public class TickRecord : APIRecord
    {
        /// <summary>
        /// Ask price in base currency.
        /// </summary>
        public double Ask { get; set; }

        /// <summary>
        /// Number of available lots to buy at given price or null if not applicable.
        /// </summary>
        public int? AskVolume { get; set; }

        /// <summary>
        /// Bid price in base currency.
        /// </summary>
        public double Bid { get; set; }

        /// <summary>
        /// Number of available lots to buy at given price or null if not applicable.
        /// </summary>
        public int? BidVolume { get; set; }

        /// <summary>
        /// The highest price of the day in base currency.
        /// </summary>
        public double High { get; set; }

        /// <summary>
        /// Price level.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// The lowest price of the day in base currency.
        /// </summary>
        public double Low { get; set; }

        /// <summary>
        /// Source of price.
        /// </summary>
        public int QuoteId { get; set; }

        /// <summary>
        /// The difference between raw ask and bid prices.
        /// </summary>
        public double SpreadRaw { get; set; }

        /// <summary>
        /// Spread representation.
        /// </summary>
        public double SpreadTable { get; set; }

        /// <summary>
        /// Symbol.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Timestamp.
        /// </summary>
        public long Timestamp { get; set; }

        public TickRecord(JSONObject body)
        {
            this.Ask = (double)body["ask"];
            this.AskVolume = (int?)body["askVolume"];
            this.Bid = (double)body["bid"];
            this.BidVolume = (int?)body["bidVolume"];
            this.High = (double)body["high"];
            this.Level = (int)body["level"];
            this.Low = (double)body["low"];
            this.QuoteId = (int)body["quoteId"];
            this.SpreadRaw = (double)body["spreadRaw"];
            this.SpreadTable = (double)body["spreadTable"];
            this.Symbol = (string)body["symbol"];
            this.Timestamp = (long)body["timestamp"];
        }
	}
}