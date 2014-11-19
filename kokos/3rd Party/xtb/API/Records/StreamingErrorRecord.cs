using System.Collections.Generic;

namespace xAPI.Records
{
    using System;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class StreamingErrorRecord : APIRecord
    {
        /// <summary>
        /// Error code.
        /// </summary>
        public string ErrorCode { get; private set; }

        /// <summary>
        /// Error description.
        /// </summary>
        public string ErrorDescription { get; private set; }

        public StreamingErrorRecord(JSONObject body)
        {
            ErrorCode = (string)body["errorCode"];
            ErrorDescription = (string)body["errorDescr"];
        }
    }
}