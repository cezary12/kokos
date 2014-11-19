using xAPI.Codes;

namespace xAPI.Commands.ReqRes
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class GetCandlesCommand : APICommand
	{
        public GetCandlesCommand(PeriodCode period, string symbol, long start, long end = 0)
            : base("getCandles")
        {
            base.AddField("period", period.Code);
            base.AddField("symbol", symbol);
            base.AddField("start", start);

            if (end != 0)
                base.AddField("end", end);
		}
	}
}