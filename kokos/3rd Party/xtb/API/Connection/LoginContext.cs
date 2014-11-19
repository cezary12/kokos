using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JSONObject = Newtonsoft.Json.Linq.JObject;

namespace xAPI.Connection
{
    /// <summary>
    /// A helper class to store loginContext. Used mainly for redirect.
    /// </summary>
    class LoginContext
    {
        /// <summary>
        /// Original TaskCompletionSource
        /// </summary>
        public System.Threading.Tasks.TaskCompletionSource<Responses.LoginResponse> TaskCompletionSource { get; private set; }

        /// <summary>
        /// Exception that was thrown after getting loginResponse
        /// </summary>
        public Errors.APIErrorResponseException Exception { get; private set; }

        /// <summary>
        /// Original command that caused the exception
        /// </summary>
        public Commands.ReqRes.APICommand Command { get; private set; }

        /// <summary>
        /// Function that creates a LoginResponse from string
        /// </summary>
        public Func<string, Responses.LoginResponse> ResponseCreator { get; private set; }

        /// <summary>
        /// JSONObject that was returned by the API
        /// </summary>
        public JSONObject JSONObject
        {
            get
            {
                return Exception.ErrorResponse;
            }
        }

        public LoginContext(System.Threading.Tasks.TaskCompletionSource<Responses.LoginResponse> taskCompletionSource, Errors.APIErrorResponseException ex, Commands.ReqRes.APICommand command, Func<string, Responses.LoginResponse> func)
        {
            this.TaskCompletionSource = taskCompletionSource;
            this.Exception = ex;
            this.Command = command;
            this.ResponseCreator = func;
        }
    }
}
