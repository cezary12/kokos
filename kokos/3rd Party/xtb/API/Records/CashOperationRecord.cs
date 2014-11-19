using System;

namespace xAPI.Records
{
    using xAPI.Codes;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class CashOperationRecord : APIRecord
    {
        /// <summary>
        /// Comment.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Operation's nominal.
        /// </summary>
        public double Nominal { get; set; }

        /// <summary>
        /// Operation time.
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// Operation type.
        /// </summary>
        public CashOperationType Type { get; set; }

        public CashOperationRecord(JSONObject body)
        {
            this.Comment = (string)body["comment"];
            this.Nominal = (double)body["nominal"];
            this.Timestamp = (long)body["timestamp"];
            this.Type = CashOperationType.FromCode((int)body["type"]);
        }
	}
}