using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Commands.Streaming
{
    public class StopProfits : APIStreamingStopCommand
    {
        public StopProfits()
            : base("stopProfits")
        {
        }
    }
}
