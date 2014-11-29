using xAPI.Codes;

namespace xAPI.Commands.ReqRes
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class GetNCandlesCommand : APICommand
	{
        public GetNCandlesCommand(PeriodCode period, string symbol, long end, long number)
            : base("getNCandles")
        {
            base.AddField("period", period.Code);
            base.AddField("symbol", symbol);
            base.AddField("end", end);
            base.AddField("number", number);
		}
	}
}