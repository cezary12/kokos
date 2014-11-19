namespace xAPI.Commands.ReqRes
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class GetTradesCommand : APICommand
    {
        public GetTradesCommand()
            : base("getTrades")
        {
        }
    }

}