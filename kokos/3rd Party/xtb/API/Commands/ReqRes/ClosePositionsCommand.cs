using System.Collections.Generic;

namespace xAPI.Commands.ReqRes
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;
    using JSONArray = Newtonsoft.Json.Linq.JArray;

    public class ClosePositionsCommand : APICommand
	{
        public ClosePositionsCommand(IEnumerable<long> positions)
            : base("closePositions")
        {
            JSONArray positionsArray = new JSONArray();

            foreach (long position in positions)
            {
                positionsArray.Add(position);
            }

            base.AddField("positions", positionsArray);
		}
	}
}