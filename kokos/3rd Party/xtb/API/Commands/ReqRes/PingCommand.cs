namespace xAPI.Commands.ReqRes
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class PingCommand : APICommand
	{
        public PingCommand()
            : base("ping")
		{
		}
	}
}