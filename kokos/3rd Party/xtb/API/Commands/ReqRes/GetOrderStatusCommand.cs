namespace xAPI.Commands.ReqRes
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class GetOrderStatusCommand : APICommand
	{
        public GetOrderStatusCommand(long order)
            : base("getOrderStatus")
        {
            base.AddField("order", order);
		}
	}
}