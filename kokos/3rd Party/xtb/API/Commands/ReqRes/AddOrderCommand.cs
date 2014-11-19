using xAPI.Codes;

namespace xAPI.Commands.ReqRes
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class AddOrderCommand : APICommand
	{
        public AddOrderCommand(string customComment, long expiration, double price, Side side, double sl, string symbol, double tp, TradeType tradeType, double volume)
            : base("addOrder")
        {
            base.AddField("customComment", customComment);
            base.AddField("expiration", expiration);
            base.AddField("price", price);
            base.AddField("side", side.Code);
            base.AddField("sl", sl);
            base.AddField("symbol", symbol);
            base.AddField("tp", tp);
            base.AddField("tradeType", tradeType.Code);
            base.AddField("volume", volume);
		}
	}
}