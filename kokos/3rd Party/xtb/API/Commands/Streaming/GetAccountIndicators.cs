using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Commands.Streaming
{
    public class GetAccountIndicators :  APIStreamingSubscribeCommand
    {
        public GetAccountIndicators(string streamSessionId)
            : base("getAccountIndicators" ,streamSessionId)
        {

        }
    }
}
