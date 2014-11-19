namespace xAPI.Commands.ReqRes
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class GetStepRulesCommand : APICommand
	{
        public GetStepRulesCommand()
            : base("getStepRules")
		{
		}
	}
}