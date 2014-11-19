namespace xAPI.Commands.ReqRes
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class GetCashOperationsHistoryCommand : APICommand
    {
        public GetCashOperationsHistoryCommand(long start, long end)
            : base("getCashOperationsHistory")
        {
            base.AddField("start", start);
            base.AddField("end", end);
        }
    }
}