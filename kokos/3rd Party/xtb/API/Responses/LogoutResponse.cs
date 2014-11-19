namespace xAPI.Responses
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class LogoutResponse : APIResponse
	{
        public LogoutResponse(string body)
            : base(body)
		{
		}
	}
}