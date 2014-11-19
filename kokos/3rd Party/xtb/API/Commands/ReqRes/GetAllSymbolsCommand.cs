namespace xAPI.Commands.ReqRes
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class GetAllSymbolsCommand : APICommand
    {
        public GetAllSymbolsCommand()
            : base("getAllSymbols")
        {
        }
    }
}