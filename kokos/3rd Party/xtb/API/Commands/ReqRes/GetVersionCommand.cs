namespace xAPI.Commands.ReqRes
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class GetVersionCommand : APICommand
    {
        public GetVersionCommand()
            : base("getVersion")
        {
        }
    }
}