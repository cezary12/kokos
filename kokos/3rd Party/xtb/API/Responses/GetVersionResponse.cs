using System;

namespace xAPI.Responses
{
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class GetVersionResponse : APIResponse
    {
        /// <summary>
        /// Current API version.
        /// </summary>
        public string Version { get; set; }

        public GetVersionResponse(string body)
            : base(body)
        {
            this.Version = (string) this.ReturnData["version"];
        }
    }
}