namespace xAPI.Responses
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class PingResponse : APIResponse
	{
        public PingResponse(string body)
            : base(body)
		{
		}
	}
}