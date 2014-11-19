namespace xAPI.Commands.ReqRes
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class ClosePositionCommand : APICommand
	{
        public ClosePositionCommand(long position, double price, double volume)
            : base("closePosition")
        {
            base.AddField("position", position);
            base.AddField("price", price);
            base.AddField("volume", volume);
		}
	}
}