using System.Collections.Generic;
using System;

namespace xAPI.Records
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class NewsRecord : APIRecord
	{
        public string Body { get; set; }

        public string Key { get; set; }

        public long Time { get; set; }

        public string Title { get; set; }

        public NewsRecord(JSONObject body)
        {
            this.Body = (string)body["body"];
            this.Key = (string)body["key"];
            this.Time = (long)body["time"];
            this.Title = (string)body["title"];
        }
    }
}