namespace xAPI.Commands.ReqRes
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class GetMarginTradeCommand : APICommand
	{
        public GetMarginTradeCommand(string symbol, double volume)
            : base("getMarginTrade")
        {
            base.AddField("symbol", symbol);
            base.AddField("volume", volume);
		}
	}
}