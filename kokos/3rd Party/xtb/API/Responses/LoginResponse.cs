using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Responses
{
    public class LoginResponse : APIResponse
    {
        public string StreamSessionId { get; private set; }

        public LoginResponse(string body)
            : base(body)
        {
            this.StreamSessionId = (string)base.JSON["streamSessionId"];
        }
    }
}
