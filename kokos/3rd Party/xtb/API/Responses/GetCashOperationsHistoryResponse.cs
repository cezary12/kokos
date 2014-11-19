using System.Collections;
using System.Collections.Generic;
using xAPI.Records;

namespace xAPI.Responses
{
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class GetCashOperationsHistoryResponse : APIResponse
	{
        /// <summary>
        /// Operations history records.
        /// </summary>
        public LinkedList<CashOperationRecord> CashOperationRecords { get; set; }

        public GetCashOperationsHistoryResponse(string body)
            : base(body)
        {
            this.CashOperationRecords = new LinkedList<CashOperationRecord>();
            JSONArray arr = (JSONArray)this.ReturnData;
            foreach (JSONObject cashOperationRecordJson in arr)
            {
                CashOperationRecord record = new CashOperationRecord(cashOperationRecordJson);
                this.CashOperationRecords.AddLast(record);
            }

        }
	}
}