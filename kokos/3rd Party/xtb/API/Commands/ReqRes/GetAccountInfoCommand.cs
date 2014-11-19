namespace xAPI.Commands.ReqRes
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class GetAccountInfoCommand : APICommand
	{
		public GetAccountInfoCommand() : base("getAccountInfo")
		{
		}
	}
}