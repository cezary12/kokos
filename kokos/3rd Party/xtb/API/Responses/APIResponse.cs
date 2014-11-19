using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xAPI.Errors;

namespace xAPI.Responses
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;
    using JSONAware = Newtonsoft.Json.Linq.JContainer;

    public abstract class APIResponse
    {
        public bool Status { get; private set; }
        public string ErrorDescr { get; private set; }
        public string ErrorCode { get; private set; }
        public string CustomTag { get; private set; }
        public JSONAware ReturnData { get; private set; }
        public JSONObject JSON { get; set; }

        public APIResponse(string body)
            : this(JSONObject.Parse(body))
        {
        }

        public APIResponse(JSONObject body)
        {            
            if (body == null)
            {
                throw new APIReplyParseException("No command body found");
            }
            else
            {
                this.Status = (bool)body["status"];
                this.ErrorCode = (string)body["errorCode"];
                this.ErrorDescr = (string)body["errorDescr"];
                this.ReturnData = (JSONAware)body["returnData"];
                this.CustomTag = (string)body["customTag"];
                this.JSON = body;
                this.JSONString = this.JSON.ToString();
            }

            if (this.Status == false)
                throw new APIErrorResponseException(this.ErrorCode, this.ErrorDescr, this.JSON);
        }

        public string JSONString { get; private set; }
    }
}
