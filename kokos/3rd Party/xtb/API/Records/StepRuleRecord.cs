using System;
using xAPI.Codes;
using System.Collections.Generic;

namespace xAPI.Records
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;
    using JSONArray = Newtonsoft.Json.Linq.JArray;    

	public class StepRuleRecord : APIRecord
	{
        /// <summary>
        /// Step rule ID.
        /// </summary>
		public long Id { get; set; }

        /// <summary>
        /// Step rule name.
        /// </summary>
		public string Name { get; set; }

        /// <summary>
        /// Step records.
        /// </summary>
        public LinkedList<StepRecord> Steps { get; set; }

        public StepRuleRecord(JSONObject body)
        {
            this.Id = (long)body["id"];
            this.Name = (string)body["name"];

            this.Steps = new LinkedList<StepRecord>();
            if (body["steps"] != null)
            {
                JSONArray stepRecordsArray = (JSONArray)body["steps"];
                foreach (JSONObject stepRecordJsonObject in stepRecordsArray)
                {
                    StepRecord stepRecord = new StepRecord(stepRecordJsonObject);
                    this.Steps.AddLast(stepRecord);
                }
            }
        }
    }
}