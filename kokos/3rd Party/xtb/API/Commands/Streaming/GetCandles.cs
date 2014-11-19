using xAPI.Codes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Commands.Streaming
{
    public class GetCandles :  APIStreamingSubscribeCommand
    {
        public GetCandles(string streamSessionId, PeriodCode period, string symbol, bool onlyComplete)
            : base("getCandles" ,streamSessionId)
        {
            this.AddField("period", period.Code);
            this.AddField("symbol", symbol);
            this.AddField("onlyComplete", onlyComplete);
        }
    }
}
