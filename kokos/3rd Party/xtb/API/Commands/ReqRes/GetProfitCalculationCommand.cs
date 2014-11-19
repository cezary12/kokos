namespace xAPI.Commands.ReqRes
{
    using xAPI.Codes;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class GetProfitCalculationCommand : APICommand
	{
        public GetProfitCalculationCommand(string symbol, double volume, double openPrice, double closePrice, Side side, TradeType tradeType)
            : base("getProfitCalculation")
        {
            base.AddField("openPrice", openPrice);
            base.AddField("closePrice", closePrice);
            base.AddField("side", side.Code);
            base.AddField("symbol", symbol);
            base.AddField("tradeType", tradeType.Code);
            base.AddField("volume", volume);
		}
	}
}