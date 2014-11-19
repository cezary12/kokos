using System.Collections;
using System.Collections.Generic;
using xAPI.Records;

namespace xAPI.Responses
{
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class GetTickPricesResponse : APIResponse
	{
        /// <summary>
        /// Tick records.
        /// </summary>
        public LinkedList<TickRecord> Ticks;

        public GetTickPricesResponse(string body)
            : base(body)
		{
            this.Ticks = new LinkedList<TickRecord>();
            JSONArray arr = (JSONArray) this.ReturnData;
			foreach (JSONObject tickRecordJson in arr)
			{
				TickRecord record = new TickRecord(tickRecordJson);
                this.Ticks.AddLast(record);
			}
		}
	}
}