using System.Collections;
using System.Collections.Generic;
using xAPI.Records;

namespace xAPI.Responses
{
    using System;
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class GetCandlesResponse : APIResponse
	{
        /// <summary>
        /// Number of decimal places.
        /// </summary>
		public long Digits { get; set; }

        /// <summary>
        /// Candle records.
        /// </summary>
        public LinkedList<CandleRecord> Candles { get; set; }

        public GetCandlesResponse(string body)
            : base(body)
		{
            this.Candles = new LinkedList<CandleRecord>();

            this.Digits = (long) this.ReturnData["digits"];
            JSONArray arr = (JSONArray) this.ReturnData["candles"];

			foreach (JSONObject candleRecordJson in arr)
			{
                CandleRecord record = new CandleRecord(candleRecordJson);
                this.Candles.AddLast(record);
			}
		}
	}
}