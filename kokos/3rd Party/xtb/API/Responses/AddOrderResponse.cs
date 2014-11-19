using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xAPI.Records;

namespace xAPI.Responses
{
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class AddOrderResponse : APIResponse
    {
        /// <summary>
        /// Order number of the add request.
        /// </summary>
        public long Order { get; set; }

        public AddOrderResponse(string body)
            : base(body)
        {
            this.Order = (long)ReturnData["order"];
        }
    }
}
