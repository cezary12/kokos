﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Commands.Streaming
{
    public class StopNews : APIStreamingStopCommand
    {
        public StopNews()
            : base("stopNews")
        {
        }
    }
}
