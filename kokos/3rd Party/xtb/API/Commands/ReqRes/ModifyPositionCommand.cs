namespace xAPI.Commands.ReqRes
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class ModifyPositionCommand : APICommand
	{
        public ModifyPositionCommand(long position, double sl, double tp)
            : base("modifyPosition")
        {
            base.AddField("position", position);
            base.AddField("sl", sl);
            base.AddField("tp", tp);
		}
	}
}