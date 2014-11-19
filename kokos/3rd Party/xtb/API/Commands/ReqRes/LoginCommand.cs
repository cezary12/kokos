using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xAPI.Utils;

namespace xAPI.Commands.ReqRes
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class LoginCommand : APICommand
    {
        public LoginCommand(Credentials credentials)
            : base("login")
        {
            base.AddField("userId", credentials.Login);
            base.AddField("password", credentials.Password);
        }
    }
}
