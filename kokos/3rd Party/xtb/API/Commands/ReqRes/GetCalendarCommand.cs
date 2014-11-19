namespace xAPI.Commands.ReqRes
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class GetCalendarCommand : APICommand
	{
        public GetCalendarCommand()
            : base("getCalendar")
		{
		}
	}
}