namespace xAPI.Commands.ReqRes
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class GetTradesHistoryCommand : APICommand
    {
        public GetTradesHistoryCommand(long start, long end)
            : base("getTradesHistory")
        {
            base.AddField("start", start);
            base.AddField("end", end);
        }
    }
}