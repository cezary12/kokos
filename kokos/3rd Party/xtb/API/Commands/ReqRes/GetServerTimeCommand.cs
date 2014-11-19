using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Commands.ReqRes
{
    public class GetServerTimeCommand : APICommand
    {
        public GetServerTimeCommand(): base("getServerTime")
        {
        }
    }
}
