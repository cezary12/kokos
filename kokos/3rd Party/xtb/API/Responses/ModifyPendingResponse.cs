using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Responses
{
    public class ModifyPendingResponse : APIResponse
    {
        public long Order { get; private set; }

        public ModifyPendingResponse(string body)
            : base(body)
        {
            this.Order = (long)base.JSON["order"];
        }
    }
}
