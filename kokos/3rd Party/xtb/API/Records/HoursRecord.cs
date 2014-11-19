using System;

namespace xAPI.Records
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class HoursRecord : APIRecord
	{
        /// <summary>
        /// Day of week.
        /// </summary>
        public int Day { get; set; }
        
        /// <summary>
        /// Start time from 00:00 in ms.
        /// </summary>
        public long FromT { get; set; }

        /// <summary>
        /// End time from 00:00 in ms.
        /// </summary>
        public long ToT { get; set; }

        public HoursRecord(JSONObject body)
        {
            this.Day = (int)body["day"];
            this.FromT = (long)body["fromT"];
            this.ToT = (long)body["toT"];
        }
	}
}