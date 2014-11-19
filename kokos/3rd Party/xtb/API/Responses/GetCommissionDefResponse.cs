using System;

namespace xAPI.Responses
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class GetCommissionDefResponse : APIResponse
	{
        /// <summary>
        /// Calculated commission in account currency, could be null if not applicable.
        /// </summary>
        public double? Commission { get; set; }

        /// <summary>
        /// Rate of exchange between account currency and instrument base currency, could be null if not applicable.
        /// </summary>
        public double? RateOfExchange { get; set; }

        public GetCommissionDefResponse(string body)
            : base(body)
		{
            this.Commission = (double?) ReturnData["commission"];
            this.RateOfExchange = (double?) ReturnData["rateOfExchange"];
		}
	}
}