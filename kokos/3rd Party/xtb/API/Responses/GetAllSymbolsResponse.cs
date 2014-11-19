using System.Collections;
using System.Collections.Generic;
using xAPI.Records;

namespace xAPI.Responses
{
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class GetAllSymbolsResponse : APIResponse
	{
        /// <summary>
        /// Symbol records.
        /// </summary>
        public LinkedList<SymbolRecord> SymbolRecords { get; set; }

        public GetAllSymbolsResponse(string body)
            : base(body)
		{
            this.SymbolRecords = new LinkedList<SymbolRecord>();
			JSONArray symbolRecords = (JSONArray) this.ReturnData;
            foreach (JSONObject symbolRecordJson in symbolRecords)
            {
                SymbolRecord symbolRecord = new SymbolRecord(symbolRecordJson);
                this.SymbolRecords.AddLast(symbolRecord);
            }
		}
	}
}