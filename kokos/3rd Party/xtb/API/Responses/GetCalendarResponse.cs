using System.Collections;
using System.Collections.Generic;
using xAPI.Records;

namespace xAPI.Responses
{
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class GetCalendarResponse : APIResponse
	{
        /// <summary>
        /// Calendar records.
        /// </summary>
        public List<CalendarRecord> CalendarRecords { get; set; }

        public GetCalendarResponse(string body)
            : base(body)
		{
            this.CalendarRecords =  new List<CalendarRecord>();
            JSONArray returnData = (JSONArray) this.ReturnData;

            foreach (JSONObject calendarRecordJson in returnData)
			{
                CalendarRecord record = new CalendarRecord(calendarRecordJson);
                this.CalendarRecords.Add(record);
			}
		}
	}
}