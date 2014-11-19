using System.Collections.Generic;

namespace xAPI.Records
{
    using System;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class StreamingCandleRecord : CandleRecord
    {
        /// <summary>
        /// Source of price.
        /// </summary>
        public long QuoteId { get; private set; }

        /// <summary>
        /// Symbol.
        /// </summary>
        public string Symbol { get; private set; }

        /// <summary>
        /// Complete flag - true if the candle for the given time range was completed.
        /// </summary>
        public bool Complete { get; private set; }

        public StreamingCandleRecord(JSONObject body)
            : base(body)
        {
            QuoteId = (long)body["quoteId"];
            Symbol = (string)body["symbol"];
            Complete = (bool)body["complete"];
        }
    }
}