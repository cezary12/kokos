namespace xAPI.Commands.ReqRes
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class LogoutCommand : APICommand
	{
		public LogoutCommand() : base("logout")
		{
		}
	}
}