using System.Collections.Generic;
using System;

namespace xAPI.Records
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class NewsTopicRecord : APIRecord
    {
        /// <summary>
        /// Body.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// News key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Time.
        /// </summary>
        public long Time { get; set; }

        /// <summary>
        /// News title.
        /// </summary>
        public string Title { get; set; }

        public NewsTopicRecord(JSONObject body)
        {
            this.Body = (string)body["body"];
            this.Key = (string)body["key"];
            this.Time = (long)body["time"];
            this.Title = (string)body["title"];
        }
    }
}