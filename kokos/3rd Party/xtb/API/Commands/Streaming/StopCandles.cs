using xAPI.Codes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Commands.Streaming
{
    public class StopCandles : APIStreamingStopCommand
    {
        public StopCandles(PeriodCode period, string symbol)
            : base("stopCandles")
        {
            this.AddField("period", period.Code);
            this.AddField("symbol", symbol);
        }
    }
}
