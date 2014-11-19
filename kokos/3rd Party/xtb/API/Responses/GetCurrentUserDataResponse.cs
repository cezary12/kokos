namespace xAPI.Responses
{
    using System;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class GetAccountInfoResponse : APIResponse
	{
        /// <summary>
        /// Unit the account is assigned to.
        /// </summary>
        public int CompanyUnit { get; set; }

        /// <summary>
        /// Account currency.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// The factor used for margin calculations. The actual value of leverage can be calculated by dividing this value by 100.
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// Indicates whether this account is an IB account.
        /// </summary>
        public bool IbAccount { get; set; }

        /// <summary>
        /// Account group.
        /// </summary>
        public double LeverageMultiplier { get; set; }
        
        /// <summary>
        /// Spread type.
        /// </summary>
        public string SpreadType { get; set; }

        public GetAccountInfoResponse(string body)
            : base(body)
        {
            this.CompanyUnit = (int) this.ReturnData["companyUnit"];
            this.Currency = (string) this.ReturnData["currency"];
            this.Group = (string) this.ReturnData["group"];
            this.IbAccount = (bool) this.ReturnData["ibAccount"];
            this.LeverageMultiplier = (double) this.ReturnData["leverageMultiplier"];
            this.SpreadType = (string) this.ReturnData["spreadType"];
        }
	}

}