using System.Collections.Generic;
using System;

namespace xAPI.Records
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;
    using JSONArray = Newtonsoft.Json.Linq.JArray;    

    public class KeepAliveRecord : APIRecord
    {
        /// <summary>
        /// Current timestamp.
        /// </summary>
        public long Timestamp { get; set; }

        public KeepAliveRecord(JSONObject body)
        {
            this.Timestamp = (long)body["timestamp"];
        }
    }
}
