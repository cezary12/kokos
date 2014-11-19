using System;
using xAPI.Responses;

namespace xAPI.Errors
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class APIErrorResponseException : APIException
    {
        public string ErrorCode { get; private set; }
        public string ErrorDescription { get; private set; }
        public JSONObject ErrorResponse { get; private set; }

        public APIErrorResponseException(string code, string description, JSONObject errorResponse)
            : base("APIErrorResponse [code " + code + ": " + description + "]")
		{
            this.ErrorCode = code;
            this.ErrorDescription = description;
            this.ErrorResponse = errorResponse;
		}
	}
}