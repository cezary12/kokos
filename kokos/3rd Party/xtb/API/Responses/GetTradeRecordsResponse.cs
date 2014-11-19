using System.Collections;
using System.Collections.Generic;
using xAPI.Records;

namespace xAPI.Responses
{
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class GetTradeRecordsResponse : APIResponse
	{
        /// <summary>
        /// Trade records.
        /// </summary>
        public LinkedList<TradeRecord> TradeRecords { get; set; }

        public GetTradeRecordsResponse(string body)
            : base(body)
		{
            this.TradeRecords = new LinkedList<TradeRecord>();
			JSONArray arr = (JSONArray) this.ReturnData;
            foreach (JSONObject tradeRecordJson in arr)
            {
                TradeRecord record = new TradeRecord(tradeRecordJson);
                this.TradeRecords.AddLast(record);
            }
		}
	}
}