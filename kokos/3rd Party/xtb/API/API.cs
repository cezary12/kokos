using Inspirel.YAMI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xAPI.Codes;
using xAPI.Commands.ReqRes;
using xAPI.Commands.Streaming;
using xAPI.Connection;
using xAPI.Errors;
using xAPI.Records;
using xAPI.Responses;
using xAPI.Utils;
using JSONObject = Newtonsoft.Json.Linq.JObject;

namespace xAPI
{
    public class API
    {
        public const string VERSION = "3.1.2";

        #region Events

        /// <summary>
        /// Delegate called on tick record arrival.
        /// </summary>
        /// <param name="tickRecord">Received tick record</param>
        public delegate void OnTick(TickRecord tickRecord);

        /// <summary>
        /// Event raised when tick is received.
        /// </summary>
        public event OnTick TickPricesRecordReceived;

        /// <summary>
        /// Delegate called on trade record arrival.
        /// </summary>
        /// <param name="tradeRecord">Received trade record</param>
        public delegate void OnTrade(StreamingTradeRecord tradeRecord);

        /// <summary>
        /// Event raised when trade record is received.
        /// </summary>
        public event OnTrade TradeRecordReceived;

        /// <summary>
        /// Delegate called on balance record arrival.
        /// </summary>
        /// <param name="balanceRecord">Received balance record</param>
        public delegate void OnAccountIndicator(IndicatorsRecord accountIndicatorRecord);

        /// <summary>
        /// Event raised when balance record is received.
        /// </summary>
        public event OnAccountIndicator AccountIndicatorsRecordReceived;

        /// <summary>
        /// Delegate called on order status record arrival.
        /// </summary>
        /// <param name="orderStatusRecord">Received order status record</param>
        public delegate void OnOrderStatus(OrderStatusRecord orderStatusRecord);

        /// <summary>
        /// Event raised when order status record is received.
        /// </summary>
        public event OnOrderStatus OrderStatusRecordReceived;

        /// <summary>
        /// Delegate called on profit record arrival.
        /// </summary>
        /// <param name="profitRecord">Received profit record</param>
        public delegate void OnProfit(ProfitRecord profitRecord);

        /// <summary>
        /// Event raised when profit record is received.
        /// </summary>
        public event OnProfit ProfitRecordReceived;

        /// <summary>
        /// Delegate called on news record arrival.
        /// </summary>
        /// <param name="newsRecord">Received news record</param>
        public delegate void OnNews(NewsRecord newsRecord);

        /// <summary>
        /// Event raised when news record is received.
        /// </summary>
        public event OnNews NewsRecordReceived;

        /// <summary>
        /// Delegate called on keep alive record arrival.
        /// </summary>
        /// <param name="keepAliveRecord">Received keep alive record</param>
        public delegate void OnKeepAlive(KeepAliveRecord keepAliveRecord);

        /// <summary>
        /// Event raised when keep alive record is received.
        /// </summary>
        public event OnKeepAlive KeepAliveRecordReceived;

        /// <summary>
        /// Delegate called on candle record arrival.
        /// </summary>
        /// <param name="candleRecord">Received candle record</param>
        public delegate void OnCandle(StreamingCandleRecord candleRecord);

        /// <summary>
        /// Event raised when candle record is received.
        /// </summary>
        public event OnCandle CandleRecordReceived;

        /// <summary>
        /// Delegate called on error record arrival.
        /// </summary>
        /// <param name="errorRecord">Received error record</param>
        public delegate void OnError(StreamingErrorRecord errorRecord);

        /// <summary>
        /// Event raised when error record is received.
        /// </summary>
        public event OnError ErrorRecordReceived;

        /// <summary>
        /// Delegate called on any record arrival.
        /// </summary>
        /// <param name="message">Received message</param>
        public delegate void OnMessage(string message);

        /// <summary>
        /// Event raised when any record is received.
        /// </summary>
        public event OnMessage RecordReceived;

        #endregion

        /// <summary>
        /// Streaming session id.
        /// </summary>
        public string StreamSessionId { get; set; }        

        /// <summary>
        /// The connector.
        /// </summary>
        private AsyncYAMIConnector connector;
        
        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="server">Server</param>
        public API(Server server) 
        {
            connector = new AsyncYAMIConnector(server, this.StreamingMessageHandler);
        }

        /// <summary>
        /// Adds new positions and pending orders.
        /// </summary>
        /// <param name="customComment">The value the customer may provide in order to retrieve it later</param>
        /// <param name="expiration">Pending order expiration time (or 0 if not set)</param>
        /// <param name="price">Trade price</param>
        /// <param name="side">Operation code</param>
        /// <param name="sl">Stop loss value (or 0.0 if not set)</param>
        /// <param name="symbol">Trade symbol</param>
        /// <param name="tp">Take profit value (or 0.0 if not set)</param>
        /// <param name="tradeType">Trade transaction type</param>
        /// <param name="volume">Trade volume</param>
        /// <returns>AddOrder response task</returns>
        public Task<AddOrderResponse> AddOrder(string customComment, long expiration, double price, Side side, double sl, string symbol, double tp, TradeType tradeType, double volume)
        {
            return connector.Execute<AddOrderResponse>(new AddOrderCommand(customComment, expiration, price, side, sl, symbol, tp, tradeType, volume), (packet) => new AddOrderResponse(packet));
        }

        /// <summary>
        /// Closes the given position.
        /// </summary>
        /// <param name="position">Number of position to be closed</param>
        /// <param name="price">Trade price</param>
        /// <param name="volume">Trade volume to be closed</param>
        /// <returns>ClosePosition response task</returns>
        public Task<ClosePositionResponse> ClosePosition(long position, double price, double volume)
        {
            return connector.Execute<ClosePositionResponse>(new ClosePositionCommand(position, price, volume), (packet) => new ClosePositionResponse(packet));
        }

        /// <summary>
        /// Closes positions from the list.
        /// </summary>
        /// <param name="positions">List of position numbers to be closed</param>
        /// <returns>ClosePositions response task</returns>
        public Task<ClosePositionsResponse> ClosePositions(IEnumerable<long> positions)
        {
            return connector.Execute<ClosePositionsResponse>(new ClosePositionsCommand(positions), (packet) => new ClosePositionsResponse(packet));
        }

        /// <summary>
        /// Deletes pending orders.
        /// </summary>
        /// <param name="order">Pending's order number to be deleted</param>
        /// <returns>DeletePending response task</returns>
        public Task<DeletePendingResponse> DeletePending(long order)
        {
            return connector.Execute<DeletePendingResponse>(new DeletePendingCommand(order), (packet) => new DeletePendingResponse(packet));
        }

        /// <summary>
        /// Returns array of all symbols available for the user.
        /// </summary>
        /// <returns>GetAllSymbols response task</returns>
        public Task<GetAllSymbolsResponse> GetAllSymbols()
        {
            return connector.Execute<GetAllSymbolsResponse>(new GetAllSymbolsCommand(), (packet) => new GetAllSymbolsResponse(packet));
        }

        /// <summary>
        /// Returns calendar with market events.
        /// </summary>
        /// <returns>GetCalendar response task</returns>
        public Task<GetCalendarResponse> GetCalendar()
        {
            return connector.Execute<GetCalendarResponse>(new GetCalendarCommand(), (packet) => new GetCalendarResponse(packet));
        }

        /// <summary>
        /// Returns candles data between given start and end dates. 
        /// If the chosen period is greater than 1 minute and the end date is not set, the last candle returned by the API can change 
        /// until the end of the period (the candle is being automatically updated every minute).
        /// </summary>
        /// <param name="period">Requested candles interval in minutes</param>
        /// <param name="symbol">Name of candle symbol</param>
        /// <param name="start">Start of chart block (rounded down to the nearest interval and excluding)</param>
        /// <param name="end">This field is optional and defines the end of candles block (rounded down to the nearest interval and excluding). If it is not set (or set to 0) then the current date is used</param>
        /// <returns>GetCandles response task</returns>
        public Task<GetCandlesResponse> GetCandles(PeriodCode period, string symbol, long start, long end = 0)
        {
            return connector.Execute<GetCandlesResponse>(new GetCandlesCommand(period, symbol, start, end), (packet) => new GetCandlesResponse(packet));
        }

        /// <summary>
        /// Returns the given number of candles data before the end date.
        /// </summary>
        /// <param name="period">Requested candles interval in minutes</param>
        /// <param name="symbol">Name of candle symbol</param>
        /// <param name="end">Defines the end timestamp of candles block (rounded down to the nearest interval)</param>
        /// <param name="number">Number of requested candles</param>
        /// <returns>GetCandles response task</returns>
        public Task<GetCandlesResponse> GetNCandles(PeriodCode period, string symbol, long end, long number)
        {
            return connector.Execute<GetCandlesResponse>(new GetNCandlesCommand(period, symbol, end, number), (packet) => new GetCandlesResponse(packet));
        }

        /// <summary>
        /// Returns array of user's financial operations.
        /// </summary>
        /// <param name="start">Time, 0 means current time for simplicity</param>
        /// <param name="end">Time, 0 means last month interval</param>
        /// <returns>GetCashOperations response task</returns>
        public Task<GetCashOperationsHistoryResponse> GetCashOperationsHistory(long start, long end)
        {
            return connector.Execute<GetCashOperationsHistoryResponse>(new GetCashOperationsHistoryCommand(start, end), (packet) => new GetCashOperationsHistoryResponse(packet));
        }

        /// <summary>
        /// Returns calculation of commission and rate of exchange. 
        /// The value is calculated as expected value and therefore might not be perfectly accurate.
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="volume">Volume</param>
        /// <returns>GetCommissionDef response task</returns>
        public Task<GetCommissionDefResponse> GetCommissionDef(string symbol, double volume)
        {
            return connector.Execute<GetCommissionDefResponse>(new GetCommissionDefCommand(symbol, volume), (packet) => new GetCommissionDefResponse(packet));
        }

        /// <summary>
        /// Returns information about account currency, and account leverage.
        /// </summary>
        /// <returns>GetCurrentUserData response task</returns>
        public Task<GetAccountInfoResponse> GetAccountInfo()
        {
            return connector.Execute<GetAccountInfoResponse>(new GetAccountInfoCommand(), (packet) => new GetAccountInfoResponse(packet));
        }

        /// <summary>
        /// Returns IBs data from the given time range.
        /// </summary>
        /// <param name="start">Start of IBs history block</param>
        /// <param name="end">End of IBs history block</param>
        /// <returns>GetIbsHistory response task</returns>
        public Task<GetIbsHistoryResponse> GetIbsHistory(long start, long end)
        {
            return connector.Execute<GetIbsHistoryResponse>(new GetIbsHistoryCommand(start, end), (packet) => new GetIbsHistoryResponse(packet));
        }

        /// <summary>
        /// Returns margin level for account.
        /// </summary>
        /// <returns>GetMarginLevel response task</returns>
        public Task<GetAccountIndicatorsResponse> GetAccountIndicators()
        {
            return connector.Execute<GetAccountIndicatorsResponse>(new GetAccountIndicatorsCommand(), (packet) => new GetAccountIndicatorsResponse(packet));
        }

        /// <summary>
        /// Returns expected margin for given instrument and volume. The value is calculated as expected margin value, and therefore might not be perfectly accurate.
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="volume">Volume</param>
        /// <returns>GetMarginTrade response task</returns>
        public Task<GetMarginTradeResponse> GetMarginTrade(string symbol, double volume)
        {
            return connector.Execute<GetMarginTradeResponse>(new GetMarginTradeCommand(symbol, volume), (packet) => new GetMarginTradeResponse(packet));
        }

        /// <summary>
        /// Returns news from trading server which were sent within specified period of time.
        /// </summary>
        /// <param name="start">Time</param>
        /// <param name="end">Time, 0 means current time for simplicity</param>
        /// <returns>GetNews response task</returns>
        public Task<GetNewsResponse> GetNews(long start, long end)
        {
            return connector.Execute<GetNewsResponse>(new GetNewsCommand(start, end), (packet) => new GetNewsResponse(packet));
        }

        /// <summary>
        /// Returns current transaction status. At any time of transaction processing client might check the status of transaction on server side. In order to do that client must provide unique order taken from addOrder invocation.
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>GetOrderStatus response task</returns>
        public Task<GetOrderStatusResponse> GetOrderStatus(long order)
        {
            return connector.Execute<GetOrderStatusResponse>(new GetOrderStatusCommand(order), (packet) => new GetOrderStatusResponse(packet));
        }

        /// <summary>
        /// Calculates estimated profit for given deal data Should be used for calculator-like apps only. 
        /// Profit for opened transactions should be taken from server, due to higher precision of server calculation.
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="volume">Volume</param>
        /// <param name="openPrice">Theoretical open price of order</param>
        /// <param name="closePrice">Theoretical close price of order</param>
        /// <param name="side">Operation code</param>
        /// <param name="tradeType">Trade transaction type</param>
        /// <returns>GetProfitCalculation response task</returns>
        public Task<GetProfitCalculationResponse> GetProfitCalculation(string symbol, double volume, double openPrice, double closePrice, Side side, TradeType tradeType)
        {
            return connector.Execute<GetProfitCalculationResponse>(new GetProfitCalculationCommand(symbol, volume, openPrice, closePrice, side, tradeType), (packet) => new GetProfitCalculationResponse(packet));
        }

        /// <summary>
        /// Returns current time on trading server.
        /// </summary>
        /// <returns>GetServerTime response task</returns>
        public Task<GetServerTimeResponse> GetServerTime()
        {
            return connector.Execute<GetServerTimeResponse>(new GetServerTimeCommand(), (packet) => new GetServerTimeResponse(packet));
        }

        /// <summary>
        /// Returns a list of step rules for DMAs.
        /// </summary>
        /// <returns>GetStepRules response task</returns>
        public Task<GetStepRulesResponse> GetStepRules()
        {
            return connector.Execute<GetStepRulesResponse>(new GetStepRulesCommand(), (packet) => new GetStepRulesResponse(packet));
        }

        /// <summary>
        /// Gets information about symbol available for the user.
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <returns>GetSymbol response task</returns>
        public Task<GetSymbolResponse> GetSymbol(string symbol)
        {
            return connector.Execute<GetSymbolResponse>(new GetSymbolCommand(symbol), (packet) => new GetSymbolResponse(packet));
        }

        /// <summary>
        /// Returns a list of TICK_RECORDs  for the given symbols (only records that changed from the given timestamp are returned). New timestamp obtained from output will be used as an argument of the next call of this command.
        /// </summary>
        /// <param name="symbols">Symbols</param>
        /// <param name="level">Price level</param>
        /// <param name="timestamp">The time from which the most recent tick should be looked for. Historical prices cannot be obtained using this parameter. It can only be used to verify whether a price has changed since the given time</param>
        /// <returns>GetTickPrices response task</returns>
        public Task<GetTickPricesResponse> GetTickPrices(IEnumerable<string> symbols, int level)
        {
            return connector.Execute<GetTickPricesResponse>(new GetTickPricesCommand(symbols, level), (packet) => new GetTickPricesResponse(packet));
        }

        /// <summary>
        /// Returns array of trades listed in orders argument.
        /// </summary>
        /// <param name="orders">Orders</param>
        /// <returns>GetTradeRecords response task</returns>
        public Task<GetTradeRecordsResponse> GetTradeRecords(IEnumerable<int> orders)
        {
            return connector.Execute<GetTradeRecordsResponse>(new GetTradeRecordsCommand(orders), (packet) => new GetTradeRecordsResponse(packet));
        }

        /// <summary>
        /// Returns array of user's trades.
        /// </summary>
        /// <returns>GetTrades response task</returns>
        public Task<GetTradesResponse> GetTrades()
        {
            return connector.Execute<GetTradesResponse>(new GetTradesCommand(), (packet) => new GetTradesResponse(packet));
        }

        /// <summary>
        /// Returns array of user's trades which were closed within specified period of time.
        /// </summary>
        /// <param name="start">Time, 0 means current time for simplicity</param>
        /// <param name="end">Time, 0 means last month interval</param>
        /// <returns>GetTradesHistory response task</returns>
        public Task<GetTradesHistoryResponse> GetTradesHistory(long start, long end)
        {
            return connector.Execute<GetTradesHistoryResponse>(new GetTradesHistoryCommand(start, end), (packet) => new GetTradesHistoryResponse(packet));
        }

        /// <summary>
        /// Returns quotes and trading times.
        /// </summary>
        /// <param name="symbols">Array of symbol names (Strings)</param>
        /// <returns>GetTradingHours response task</returns>
        public Task<GetTradingHoursResponse> GetTradingHours(IEnumerable<string> symbols)
        {
            return connector.Execute<GetTradingHoursResponse>(new GetTradingHoursCommand(symbols), (packet) => new GetTradingHoursResponse(packet));
        }

        /// <summary>
        /// Returns the current API version.
        /// </summary>
        /// <returns>GetVersion response task</returns>
        public Task<GetVersionResponse> GetVersion()
        {
            return connector.Execute<GetVersionResponse>(new GetVersionCommand(), (packet) => new GetVersionResponse(packet));
        }

        /// <summary>
        /// Logs user in.
        /// </summary>
        /// <param name="credentials">User credentials</param>
        /// <returns>Login response task</returns>
        public Task<LoginResponse> Login(Credentials credentials)
        {
             var task = connector.Execute<LoginResponse>(new LoginCommand(credentials), (packet) => {
                // Intercept stream session id
                var loginResponse = new LoginResponse(packet);
                StreamSessionId = loginResponse.StreamSessionId;
                this.connector.StreamSessionId = this.StreamSessionId;
                return loginResponse;
            });

            return task;
        }

        /// <summary>
        /// Logs user out.
        /// </summary>
        /// <returns>Logour response task</returns>
        public Task<LogoutResponse> Logout()
        {
            return connector.Execute<LogoutResponse>(new LogoutCommand(), (packet) => new LogoutResponse(packet));
        }

        /// <summary>
        /// Modifies the given pending order.
        /// </summary>
        /// <param name="order">Pending's order number to be modified</param>
        /// <param name="price">New pending order price</param>
        /// <param name="sl">Stop loss value (or 0.0 if not set)</param>
        /// <param name="tp">Take profit value (or 0.0 if not set)</param>
        /// <param name="expiration">New pending order expiration time (or 0 if not set)</param>
        /// <param name="customComment">The value the customer may provide in order to retrieve it later</param>
        /// <returns>ModifyPending response task</returns>
        public Task<ModifyPendingResponse> ModifyPending(long order, double price, double sl, double tp, long expiration, string customComment)
        {
            return connector.Execute<ModifyPendingResponse>(new ModifyPendingCommand(order, price, sl, tp, expiration, customComment), (packet) => new ModifyPendingResponse(packet));
        }

        /// <summary>
        /// Modifies the given position.
        /// </summary>
        /// <param name="position">Number of position to be modified</param>
        /// <param name="sl">Stop loss value (or 0.0 if not set)</param>
        /// <param name="tp">Take profit value (or 0.0 if not set)</param>
        /// <param name="customComment">The value the customer may provide in order to retrieve it later</param>
        /// <returns>ModifyPosition response task</returns>
        public Task<ModifyPositionResponse> ModifyPosition(long position, double sl, double tp)
        {
            return connector.Execute<ModifyPositionResponse>(new ModifyPositionCommand(position, sl, tp), (packet) => new ModifyPositionResponse(packet));
        }

        /// <summary>
        /// Regularly calling this function is enough to refresh the internal state of all the components in the system. It is recommended that any application that does not execute other commands, should call this command at least once every 10 minutes. Please note that the streaming counterpart of this function is called getKeepAlive.
        /// </summary>
        /// <returns>Ping response task</returns>
        public Task<PingResponse> Ping()
        {
            return connector.Execute<PingResponse>(new PingCommand(), (packet) => new PingResponse(packet));
        }

        /// <summary>
        /// Manually send message to API server.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task<string> Send(string message)
        {
            return connector.Execute(message);
        }

        /// <summary>
        /// Sends streaming command to the server or throws exception if user is not logged in.
        /// </summary>
        /// <param name="apiStreamingCommand">Streaming command</param>
        private void SendStreamingCommand(APIStreamingCommand apiStreamingCommand)
        {
            if (StreamSessionId != null)
                connector.Execute(apiStreamingCommand);
            else
                throw new APIException("You must login to perform any streaming command.");
        }

        /// <summary>
        /// Allows to get actual account indicators values in real-time, as soon as they are available in the system.
        /// </summary>
        public void SubscribeAccountIndicators()
        {
            SendStreamingCommand(new GetAccountIndicators(StreamSessionId));
        }

        /// <summary>
        /// Unsubscribes from account indicators records.
        /// </summary>
        public void StopAccountIndicators()
        {
            SendStreamingCommand(new StopAccountIndicators());
        }

        /// <summary>
        /// Subscribes for API chart candles. The interval of every candle is 1 minute. A new candle arrives every minute.
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="period">Requested candles interval in minutes</param>
        /// <param name="onlyComplete">If true, only fully completed candles are transmitted.</param>
        public void SubscribeCandles(PeriodCode period, string symbol, bool onlyComplete)
        {
            SendStreamingCommand(new GetCandles(StreamSessionId, period, symbol, onlyComplete));
        }
        
        /// <summary>
        /// Unsubscribes from API chart candles.
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="period">Candles interval in minutes</param>
        public void StopCandles(PeriodCode period, string symbol)
        {
            SendStreamingCommand(new StopCandles(period, symbol));
        }

        /// <summary>
        /// Subscribes for 'keep alive' messages. A new 'keep alive' message is sent by the API every 3 seconds.
        /// </summary>
        public void SubscribeKeepAlive()
        {
            SendStreamingCommand(new GetKeepAlive(StreamSessionId));            
        }

        /// <summary>
        ///  Unsubscribes from 'keep alive' messages.
        /// </summary>
        public void StopKeepAlive()
        {
            SendStreamingCommand(new StopKeepAlive());
        }

        /// <summary>
        /// Subscribes for news.
        /// </summary>
        public void SubscribeNews()
        {
            SendStreamingCommand(new GetNews(StreamSessionId)); 
        }

        /// <summary>
        /// Unsubscribes from news.
        /// </summary>
        public void StopNews()
        {
            SendStreamingCommand(new StopNews());
        }

        /// <summary>
        /// Allows to get status for sent order requests in real-time, as soon as it is available in the system.
        /// </summary>
        public void SubscribeOrderStatus()
        {
            SendStreamingCommand(new GetOrderStatus(StreamSessionId));
        }

        /// <summary>
        /// Unsubscribes order status.
        /// </summary>
        public void StopOrderStatus()
        {
            SendStreamingCommand(new StopOrderStatus());
        }

        /// <summary>
        /// Subscribes for profits.
        /// </summary>
        public void SubscribeProfits()
        {
            SendStreamingCommand(new GetProfits(StreamSessionId));
        }

        /// <summary>
        /// Unsubscribes from profits.
        /// </summary>
        public void StopProfits()
        {
            SendStreamingCommand(new StopProfits());
        }

        /// <summary>
        /// Establishes subscription for quotations and allows to obtain the relevant information in real-time, as soon as it is available in the system. 
        /// The SubscribeTickPrices command can be invoked many times for the same symbol, but only one subscription for a given symbol will be created.
        /// </summary>
        /// <param name="symbols">List of symbols on which the subscription should be initiated.</param>
        /// <param name="minArrivalTime">Min arrival time (optional)</param>
        /// <param name="maxLevel">Max level (optional)</param>
        public void SubscribeTickPrices(IEnumerable<string> symbols, int? minArrivalTime = null, int? maxLevel = null)
        {
            SendStreamingCommand(new GetTickPrices(StreamSessionId, symbols, minArrivalTime, maxLevel));
        }

        /// <summary>
        /// Unsubscribes from tick prices.
        /// </summary>
        /// <param name="symbols">List of symbols on which the subscription should be stopped.</param>
        public void StopTickPrices(IEnumerable<string> symbols)
        {
            SendStreamingCommand(new StopTickPrices(symbols));
        }

        /// <summary>
        /// Establishes subscription for user trade status data and allows to obtain the relevant information in real-time, as soon as it is available in the system.
        /// </summary>
        public void SubscribeTrades()
        {
            SendStreamingCommand(new GetTrades(StreamSessionId));
        }

        /// <summary>
        /// Unsubscribes from trades.
        /// </summary>
        public void StopTrades()
        {
            SendStreamingCommand(new StopTrades());
        }

        /// <summary>
        /// Handler for streaming message.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="args">Args</param>
        private void StreamingMessageHandler(object sender, IncomingMessageArgs args)
        {
            var message = args.Message.Parameters.GetString("update");
            JSONObject responseBody = (JSONObject)JSONObject.Parse(message);
            string commandName = responseBody["command"].ToString();

            if (RecordReceived != null)
                RecordReceived.Invoke(message);

            switch (commandName)
            {
                case "tickPrices":
                    TickRecord tickRecord = new TickRecord((JSONObject)responseBody["data"]);
                    if (TickPricesRecordReceived != null)
                        TickPricesRecordReceived.Invoke(tickRecord);
                    break;
                case "trade":
                    StreamingTradeRecord tradeRecord = new StreamingTradeRecord((JSONObject)responseBody["data"]);
                    if (TradeRecordReceived != null)
                        TradeRecordReceived.Invoke(tradeRecord);
                    break;
                case "indicators":
                    IndicatorsRecord balanceRecord = new IndicatorsRecord((JSONObject)responseBody["data"]);
                    if (AccountIndicatorsRecordReceived != null)
                        AccountIndicatorsRecordReceived.Invoke(balanceRecord);
                    break;
                case "orderStatus":
                    OrderStatusRecord tradeStatusRecord = new OrderStatusRecord((JSONObject)responseBody["data"]);
                    if (OrderStatusRecordReceived != null)
                        OrderStatusRecordReceived.Invoke(tradeStatusRecord);
                    break;
                case "profit":
                    ProfitRecord profitRecord = new ProfitRecord((JSONObject)responseBody["data"]);
                    if (ProfitRecordReceived != null)
                        ProfitRecordReceived.Invoke(profitRecord);
                    break;
                case "news":
                    NewsRecord newsRecord = new NewsRecord((JSONObject)responseBody["data"]);
                    if (NewsRecordReceived != null)
                        NewsRecordReceived.Invoke(newsRecord);
                    break;
                case "keepAlive":
                    KeepAliveRecord keepAliveRecord = new KeepAliveRecord((JSONObject)responseBody["data"]);
                    if (KeepAliveRecordReceived != null)
                        KeepAliveRecordReceived.Invoke(keepAliveRecord);
                    break;
                case "candle":
                    StreamingCandleRecord candleRecord = new StreamingCandleRecord((JSONObject)responseBody["data"]);
                    if (CandleRecordReceived != null)
                        CandleRecordReceived.Invoke(candleRecord);
                    break;
                case "error":
                    StreamingErrorRecord streamingErrorRecord = new StreamingErrorRecord((JSONObject)responseBody["data"]);
                    if (ErrorRecordReceived != null)
                        ErrorRecordReceived.Invoke(streamingErrorRecord);
                    break;
                default:
                    throw new APICommunicationException("Unknown streaming record received");
            }
        }
    }
}
