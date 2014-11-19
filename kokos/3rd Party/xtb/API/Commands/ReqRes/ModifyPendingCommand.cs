namespace xAPI.Commands.ReqRes
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class ModifyPendingCommand : APICommand
	{
        public ModifyPendingCommand(long order, double price, double sl, double tp, long expiration, string customComment)
            : base("modifyPending")
        {
            base.AddField("order", order);
            base.AddField("price", price);
            base.AddField("sl", sl);
            base.AddField("tp", tp);
            base.AddField("expiration", expiration);
            base.AddField("customComment", customComment);
		}
	}
}