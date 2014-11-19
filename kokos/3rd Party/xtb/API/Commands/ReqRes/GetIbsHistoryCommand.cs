namespace xAPI.Commands.ReqRes
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class GetIbsHistoryCommand : APICommand
	{
        public GetIbsHistoryCommand(long start, long end)
            : base("getIbsHistory")
        {
            base.AddField("start", start);
            base.AddField("end", end);
		}
	}
}