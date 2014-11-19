using System;

namespace xAPI.Responses
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class GetAccountIndicatorsResponse : APIResponse
	{
        /// <summary>
        /// Balance in account currency.
        /// </summary>
        public double Balance { get; set; }

        /// <summary>
        /// Credit.
        /// </summary>
        public double Credit { get; set; }

        /// <summary>
        /// User currency.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Sum of balance and all profits in account currency.
        /// </summary>
        public double Equity { get; set; }

        /// <summary>
        /// Margin requirements in account currency.
        /// </summary>
        public double Margin { get; set; }

        /// <summary>
        /// Free margin in account currency.
        /// </summary>
        public double MarginFree { get; set; }

        /// <summary>
        /// Margin level percentage.
        /// </summary>
        public double MarginLevel { get; set; }

        public GetAccountIndicatorsResponse(string body)
            : base(body)
		{
            this.Balance = (double) this.ReturnData["balance"];
            this.Credit = (double) this.ReturnData["credit"];
            this.Currency = (string) this.ReturnData["currency"];
            this.Equity = (double) this.ReturnData["equity"];
            this.Margin = (double) this.ReturnData["margin"];
            this.MarginFree = (double )this.ReturnData["marginFree"];
            this.MarginLevel = (double) this.ReturnData["marginLevel"];
		}
	}
}