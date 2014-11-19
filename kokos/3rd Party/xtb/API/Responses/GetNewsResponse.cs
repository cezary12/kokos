using System.Collections;
using System.Collections.Generic;
using xAPI.Records;

namespace xAPI.Responses
{
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class GetNewsResponse : APIResponse
	{
        /// <summary>
        /// News topic records.
        /// </summary>
        public LinkedList<NewsTopicRecord> NewsTopicRecords { get; set; }

        public GetNewsResponse(string body)
            : base(body)
		{
            this.NewsTopicRecords = new LinkedList<NewsTopicRecord>();

			JSONArray arr = (JSONArray) this.ReturnData;
			foreach (JSONObject newsTopicRecordJson in arr)
			{
                NewsTopicRecord record = new NewsTopicRecord(newsTopicRecordJson);
                this.NewsTopicRecords.AddLast(record);
			}
		}
	}
}