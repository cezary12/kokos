using System;

namespace xAPI.Responses
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class GetProfitCalculationResponse : APIResponse
	{
        /// <summary>
        /// Profit in account currency.
        /// </summary>
        public double Profit { get; set; }

        public GetProfitCalculationResponse(string body)
            : base(body)
		{
            this.Profit = (double) this.ReturnData["profit"];
		}
	}
}