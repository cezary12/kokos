namespace xAPI.Commands.ReqRes
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class GetNewsCommand : APICommand
	{
        public GetNewsCommand(long start, long end)
            : base("getNews")
        {
            base.AddField("start", start);
            base.AddField("end", end);
		}
	}

}