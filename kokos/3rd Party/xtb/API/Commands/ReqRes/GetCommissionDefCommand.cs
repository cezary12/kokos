namespace xAPI.Commands.ReqRes
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class GetCommissionDefCommand : APICommand
	{
        public GetCommissionDefCommand(string symbol, double volume)
            : base("getCommissionDef")
        {
            base.AddField("symbol", symbol);
            base.AddField("volume", volume);
		}
	}
}