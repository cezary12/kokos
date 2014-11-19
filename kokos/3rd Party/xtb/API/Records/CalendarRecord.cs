using System;

namespace xAPI.Records
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class CalendarRecord : APIRecord
    {
        /// <summary>
        /// Two letter country code.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Market value (current), empty before time of release of this value (time from "time" record).
        /// </summary>
        public string Current { get; set; }

        /// <summary>
        /// Forecasted value.
        /// </summary>
        public string Forecast { get; set; }

        /// <summary>
        /// Impact on market.
        /// </summary>
        public int Impact { get; set; }

        /// <summary>
        /// Information period.
        /// </summary>
        public string Period { get; set; }

        /// <summary>
        /// Value from previous information release.
        /// </summary>
        public string Previous { get; set; }

        /// <summary>
        /// Time, when the information will be released (in this time empty "current" value should be changed with exact released value).
        /// </summary>
        public long Time { get; set; }

        /// <summary>
        /// Name of the indicator for which values will be released.
        /// </summary>
        public string Title { get; set; }

        public CalendarRecord(JSONObject body)
        {
            this.Country = (string)body["country"];
            this.Current = (string)body["current"];
            this.Forecast = (string)body["forecast"];
            this.Impact = (int)body["impact"];
            this.Period = (string)body["period"];
            this.Previous = (string)body["previous"];
            this.Time = (long)body["time"];
            this.Title = (string)body["title"];
        }
    }
}