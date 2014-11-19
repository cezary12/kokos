namespace xAPI.Commands.ReqRes
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class GetSymbolCommand : APICommand
	{
		public GetSymbolCommand(string symbol) : base("getSymbol")
		{
            base.AddField("symbol", symbol);
		}
	}
}