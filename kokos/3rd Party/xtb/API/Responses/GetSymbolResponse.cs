using xAPI.Records;

namespace xAPI.Responses
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class GetSymbolResponse : APIResponse
	{
        /// <summary>
        /// Symbol record.
        /// </summary>
		public SymbolRecord Symbol { get; set; }

        public GetSymbolResponse(string body)
            : base(body)
		{
            this.Symbol = new SymbolRecord((JSONObject) this.ReturnData);
		}

	}
}