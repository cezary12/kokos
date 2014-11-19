using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xAPI.Commands.ReqRes;
using xAPI.Responses;

namespace xAPI.Connection
{
    interface IAsyncConnector
    {
        Task<Y> Execute<Y>(APICommand command, Func<string, Y> ctor, TaskCompletionSource<Y> task) where Y : APIResponse;
    }
}
