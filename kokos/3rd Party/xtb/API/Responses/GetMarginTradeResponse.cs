using System;

namespace xAPI.Responses
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class GetMarginTradeResponse : APIResponse
	{
        /// <summary>
        /// Calculated margin in account currency.
        /// </summary>
        public double Margin { get; set; }

        public GetMarginTradeResponse(string body)
            : base(body)
		{
            this.Margin = (double) this.ReturnData["margin"];
		}
	}
}