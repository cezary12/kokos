using System.Collections;
using System.Collections.Generic;
using xAPI.Records;

namespace xAPI.Responses
{
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class GetStepRulesResponse : APIResponse
	{
        /// <summary>
        /// Step rule records.
        /// </summary>
        public LinkedList<StepRuleRecord> StepRulesRecords { get; set; }

        public GetStepRulesResponse(string body)
            : base(body)
		{
            this.StepRulesRecords = new LinkedList<StepRuleRecord>();

            JSONArray stepRulesRecords = (JSONArray)this.ReturnData;
            foreach (JSONObject stepRuleRecordJson in stepRulesRecords)
            {
                StepRuleRecord stepRulesRecord = new StepRuleRecord(stepRuleRecordJson);
                this.StepRulesRecords.AddLast(stepRulesRecord);
            }
		}
	}

}