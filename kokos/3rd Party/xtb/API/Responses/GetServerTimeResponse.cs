using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Responses
{
    public class GetServerTimeResponse : APIResponse
    {
        /// <summary>
        /// Time.
        /// </summary>
        public long Time { get; set; }

        /// <summary>
        /// Time described in form set on server (local time of server).
        /// </summary>
        public string TimeString { get; set; }

        public GetServerTimeResponse(string body) : base(body)
        {
            this.Time = (long) this.ReturnData["time"];
            this.TimeString = (string) this.ReturnData["timeString"];
        }
    }
}
