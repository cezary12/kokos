using Inspirel.YAMI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xAPI.Commands;
using xAPI.Commands.ReqRes;
using xAPI.Commands.Streaming;
using xAPI.Errors;
using xAPI.Records;
using xAPI.Responses;
using xAPI.Utils;
using JSONObject = Newtonsoft.Json.Linq.JObject;

namespace xAPI.Connection
{
    class AsyncYAMIConnector : IAsyncConnector, IConnector
    {
        /// <summary>
        /// YAMI agent.
        /// </summary>
        private Agent agent;

        /// <summary>
        /// YAMI session id.
        /// </summary>
        private string YamiSessionId { get; set; }

        /// <summary>
        /// Server.
        /// </summary>
        private Server Server { get; set; }

        /// <summary>
        /// Stream session id for current session.
        /// </summary>
        public string StreamSessionId { get; set; }

        /// <summary>
        /// Queue that contains messages to be sent.
        /// </summary>
        BlockingCollection<Action> outputQueue = new BlockingCollection<Action>();

        /// <summary>
        /// Thread used to send messages.
        /// </summary>
        private Thread sendingThread;

        /// <summary>
        /// Priority - paramter for YAMI.
        /// </summary>
        private int PRIORITY = 0;

        /// <summary>
        /// Auto reconnect - parameter for YAMI.
        /// </summary>
        private const bool AUTO_RECONNECT = false;

        /// <summary>
        /// Informs about the connection state.
        /// </summary>
        private bool connected = false;

        /// <summary>
        /// Redirects counter.
        /// </summary>
        private int redirectCounter = 0;

        /// <summary>
        /// Last command timestamp (used to calculate interval between each command).
        /// </summary>
        private long lastCommandTimestamp = 0;

        /// <summary>
        /// Max number of redirects.
        /// </summary>
        private const int MAX_REDIRECT_NUMBER = 3;

        /// <summary>
        /// Connect timeout.
        /// </summary>
        private const int CONNECT_TIMEOUT_IN_MILLIS = 5000;

        /// <summary>
        /// Delay between each command sent to the server in milliseconds.
        /// </summary>
        private const long COMMAND_TIME_SPACE = 200;

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="server">Server</param>
        /// <param name="streamingCallback">Streaming callback</param>
        public AsyncYAMIConnector(Server server, IncomingMessageHandler streamingCallback)
        {
            this.Server = server;
            YamiSessionId = "";
            InitYAMIAgent(streamingCallback);
            InitSendingThread();
        }

        /// <summary>
        /// Initialises sending thread.
        /// </summary>
        private void InitSendingThread()
        {
            sendingThread = new Thread(new ThreadStart(SendMessagesWithThrottle));
            sendingThread.IsBackground = true;
            sendingThread.Start();
        }

        /// <summary>
        /// Initialises YAMI agent.
        /// </summary>
        /// <param name="streamingCallback"></param>
        private void InitYAMIAgent(IncomingMessageHandler streamingCallback)
        {
            Parameters options = new Parameters();
            options.SetInteger(OptionNames.TCP_CONNECT_TIMEOUT, CONNECT_TIMEOUT_IN_MILLIS);
            this.agent = new Agent(options);
            agent.RegisterObject("*", streamingCallback);
        }

        /// <summary>
        /// Thread used to send messages with throttle.
        /// </summary>
        public void SendMessagesWithThrottle()
        {
            while (true)
            {
                var sendAction = outputQueue.Take();

                long currentTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                long interval = currentTimestamp - lastCommandTimestamp;

                if (interval < COMMAND_TIME_SPACE)
                {
                    Thread.Sleep((int)(COMMAND_TIME_SPACE - interval));
                }

                sendAction();

                this.lastCommandTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;                
            }
        }

        /// <summary>
        /// Executes command.
        /// </summary>
        /// <param name="command">Command</param>
        /// <returns>Task<string></returns>
        public Task<string> Execute(string command)
        {
            var taskSource = new TaskCompletionSource<string>();
            Send(command, (message) =>
                {
                    taskSource.SetResult(message);
                });
            return taskSource.Task;
        }

        /// <summary>
        /// Executes command.
        /// </summary>
        /// <typeparam name="Y">Response type</typeparam>
        /// <param name="command">Command</param>
        /// <param name="ctor">Response object constructor</param>
        /// <param name="taskSource">Task source</param>
        /// <returns>Task<Response type></returns>
        public Task<Y> Execute<Y>(APICommand command, Func<string, Y> ctor, TaskCompletionSource<Y> taskSource = null) where Y : APIResponse
        {

            if (taskSource==null)
                taskSource = new TaskCompletionSource<Y>();

            ResetRedirectCounterIfFirstLoginTry(command, taskSource);

            try
            {
                Send(command.JSONString, ResponseMessageHandler<Y>(command, ctor, taskSource));
            }
            catch (APICommunicationException ex)
            {
                taskSource.SetException(ex);
            }

            return taskSource.Task;
        }

        /// <summary>
        /// Handles the response from the server.
        /// </summary>
        /// <typeparam name="Y">Response type</typeparam>
        /// <param name="command">Command</param>
        /// <param name="ctor">Response object constructor</param>
        /// <param name="taskSource">Task source</param>
        /// <returns>Action<string></returns>
        private Action<string> ResponseMessageHandler<Y>(APICommand command, Func<string, Y> ctor, TaskCompletionSource<Y> taskSource) where Y : APIResponse
        {
            return (message) =>
            {
                try
                {
                    taskSource.SetResult(ctor(message));
                }
                catch (APIErrorResponseException ex)
                {
                    if (ShouldRedirect(command, ex))
                    {
                        var loginContext = new LoginContext(taskSource as TaskCompletionSource<LoginResponse>, ex, command, ctor as Func<string, LoginResponse>);
                        ExecuteRedirect(loginContext);
                    }
                    else if (IsRedirectRecord(ex.ErrorResponse)) {
                        taskSource.SetException(new APICommunicationException("Too many redirects.")); 
                    } else {
                        taskSource.SetException(ex);
                    }
                }
            };
        }

        /// <summary>
        /// Verifies if should redirect.
        /// </summary>
        /// <param name="command">Command</param>
        /// <param name="ex">Exception</param>
        /// <returns>Bool</returns>
        private bool ShouldRedirect(APICommand command, APIErrorResponseException ex)
        {
            return command is LoginCommand && redirectCounter <= MAX_REDIRECT_NUMBER && IsRedirectRecord(ex.ErrorResponse);
        }

        /// <summary>
        /// Resets redirect counter if needed.
        /// </summary>
        /// <typeparam name="Y">Response type</typeparam>
        /// <param name="command">Command</param>
        /// <param name="taskSource">Task source</param>
        private void ResetRedirectCounterIfFirstLoginTry<Y>(APICommand command, TaskCompletionSource<Y> taskSource)
        {
            if (command is LoginCommand && taskSource == null)
                redirectCounter = 0;
        }

        /// <summary>
        /// Executes the redirect.
        /// </summary>
        /// <param name="loginContext">Login context</param>
        private void ExecuteRedirect(LoginContext loginContext)
        {
            var address = (string)loginContext.JSONObject["redirect"]["endpoint"];
            this.redirectCounter = this.redirectCounter + 1;
            this.Disconnect();
            this.Server = this.Server.WithURI(address);
            this.Execute(loginContext.Command, loginContext.ResponseCreator, loginContext.TaskCompletionSource);
        }

        /// <summary>
        /// Verifies if the given JSONObject is a redirect record.
        /// </summary>
        /// <param name="jsonObject">JSONObject</param>
        /// <returns>Bool</returns>
        private bool IsRedirectRecord(JSONObject jsonObject)
        {
            return (bool)jsonObject["status"] == false && jsonObject["redirect"] != null && jsonObject["redirect"]["endpoint"] != null; 
        }

        /// <summary>
        /// Executes streaming commands.
        /// </summary>
        /// <param name="command">Command</param>
        public void Execute(APIStreamingCommand command)
        {
            Send(command.JSONString, null);
        }

        /// <summary>
        /// Helper function to create a handler for messages incoming from a YAMI server.
        /// </summary>
        /// <param name="callback">Callback</param>
        /// <returns>Outgoing Message Handler</returns>
        private OutgoingMessageHandler CreateMessageHandler(Action<string> callback) 
        {
            return (sender) =>
            {
                if (sender.State == OutgoingMessage.MessageState.REPLIED)
                {
                    var reply = sender.Reply.GetString("reply");
                    //Console.WriteLine("RECEIVED DATA FROM JAMNIK! " + reply);
                    this.YamiSessionId = sender.Reply.GetString("sessionName");
                    if (callback != null)
                        callback(reply);
                }
                else if (sender.State == OutgoingMessage.MessageState.ABANDONED)
                {
                    Console.WriteLine("Disconnected from server.");
                }
            };
        }

        /// <summary>
        /// Sends the message with the given callback as parameter.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="callback">Callback</param>
        private void Send(string message, Action<string> callback = null)
        {
            Parameters parameters = new Parameters();
            parameters.SetString("command", message);  
            var handler = CreateMessageHandler(callback);
            try
            {
                if (!IsConnected())
                {
                    Connect(Server);
                }

                if (callback == null)                    
                   outputQueue.Add( () => agent.SendOneWay(Server.URI, StreamSessionId, "", parameters));
                else
                    outputQueue.Add(() => agent.Send(handler, Server.URI, YamiSessionId, "", parameters, PRIORITY, AUTO_RECONNECT));
            }
            catch (APICommunicationException e) {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Allows to check the connection state.
        /// </summary>
        /// <returns>Bool</returns>
        public bool IsConnected()
        {
            return connected;
        }

        /// <summary>
        /// Establishes a connection to the YAMI server.
        /// </summary>
        /// <param name="server">Server</param>
        public void Connect(Server server)
        {
            try
            {
                agent.OpenConnection(server.URI);
                this.connected = true;
            }
            catch (YAMIIOException)
            {
                this.Server = Servers.GetBackup(server);
                Connect(this.Server);
            }
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        public void Disconnect()
        {
            this.agent.CloseConnection(Server.URI);
            this.connected = false;
            this.YamiSessionId = "";
        }
    }
}
