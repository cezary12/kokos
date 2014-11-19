namespace xAPI.Commands.ReqRes
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class GetAccountIndicatorsCommand : APICommand
	{
		public GetAccountIndicatorsCommand() : base("getAccountIndicators")
		{
		}
	}
}