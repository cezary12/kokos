using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xAPI.Records
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class StepRecord : APIRecord
    {
        /// <summary>
        /// Lower border of the volume range.
        /// </summary>
        public double FromValue { get; set; }

        /// <summary>
        /// LotStep value in the given volume range.
        /// </summary>
        public double Step { get; set; }

        public StepRecord(JSONObject body)
        {
            this.FromValue = (double)body["fromValue"];
            this.Step = (double)body["step"];
        }
    }
}
