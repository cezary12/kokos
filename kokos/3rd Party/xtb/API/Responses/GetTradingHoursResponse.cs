using System.Collections;
using System.Collections.Generic;
using xAPI.Records;

namespace xAPI.Responses
{
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class GetTradingHoursResponse : APIResponse
	{
        /// <summary>
        /// Trading hours records.
        /// </summary>
        public LinkedList<TradingHoursRecord> TradingHoursRecords { get; set; }

        public GetTradingHoursResponse(string body)
            : base(body)
        {
            this.TradingHoursRecords = new LinkedList<TradingHoursRecord>();
            JSONArray arr = (JSONArray)this.ReturnData;
            foreach (JSONObject tradingHoursRecordJson in arr)
            {
                TradingHoursRecord record = new TradingHoursRecord(tradingHoursRecordJson);
                this.TradingHoursRecords.AddLast(record);
            }
        }
	}
}