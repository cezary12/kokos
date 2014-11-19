namespace xAPI.Commands.ReqRes
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class DeletePendingCommand : APICommand
	{
        public DeletePendingCommand(long order)
            : base("deletePending")
        {
            base.AddField("order", order);
		}
	}
}