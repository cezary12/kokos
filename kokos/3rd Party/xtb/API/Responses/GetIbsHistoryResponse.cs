using System.Collections;
using System.Collections.Generic;
using xAPI.Records;

namespace xAPI.Responses
{
    using System;
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class GetIbsHistoryResponse : APIResponse
	{
        /// <summary>
        /// IB records.
        /// </summary>
        public LinkedList<IbRecord> IbRecords { get; set; }

        public GetIbsHistoryResponse(string body)
            : base(body)
		{
            JSONArray arr = (JSONArray) this.ReturnData;

            this.IbRecords = new LinkedList<IbRecord>();

			foreach (JSONObject ibRecordJson in arr)
			{
                IbRecord record = new IbRecord(ibRecordJson);
                this.IbRecords.AddLast(record);
			}
		}
	}
}