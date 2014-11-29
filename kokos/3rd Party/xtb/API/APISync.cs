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
    public class APISync : API
    {
        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="server">Server</param>
        public APISync(Server server)
            : base(server)
        {
        }

        /// <summary>
        /// Helper wrapper for synchronous command invocation.
        /// </summary>
        /// <typeparam name="T">APIResponse type</typeparam>
        /// <param name="task">Asynchronous task</param>
        /// <returns>APIResponse</returns>
        public T SyncCommandExecuteWrapper<T>(Task<T> task) where T : APIResponse
        {
            try
            {
                task.Wait();
                return task.Result;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// Logs user in.
        /// </summary>
        /// <param name="credentials">User credentials</param>
        /// <returns>Login response task</returns>
        new public LoginResponse Login(Credentials credentials)
        {
            return SyncCommandExecuteWrapper<LoginResponse>(base.Login(credentials));
        }

        /// <summary>
        /// Logs user out.
        /// </summary>
        /// <returns>Logour response task</returns>
        new public LogoutResponse Logout()
        {
            return SyncCommandExecuteWrapper<LogoutResponse>(base.Logout());
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
        new public AddOrderResponse AddOrder(string customComment, long expiration, double price, Side side, double sl, string symbol, double tp, TradeType tradeType, double volume)
        {
            return SyncCommandExecuteWrapper<AddOrderResponse>(base.AddOrder(customComment, expiration, price, side, sl, symbol, tp, tradeType, volume));            
        }

        /// <summary>
        /// Closes the given position.
        /// </summary>
        /// <param name="position">Number of position to be closed</param>
        /// <param name="price">Trade price</param>
        /// <param name="volume">Trade volume to be closed</param>
        /// <returns>ClosePosition response task</returns>
        new public ClosePositionResponse ClosePosition(long position, double price, double volume)
        {
            return SyncCommandExecuteWrapper<ClosePositionResponse>(base.ClosePosition(position, price, volume));
        }

        /// <summary>
        /// Closes positions from the list.
        /// </summary>
        /// <param name="positions">List of position numbers to be closed</param>
        /// <returns>ClosePositions response task</returns>
        new public ClosePositionsResponse ClosePositions(IEnumerable<long> positions)
        {
            return SyncCommandExecuteWrapper<ClosePositionsResponse>(base.ClosePositions(positions));
        }

        /// <summary>
        /// Deletes pending orders.
        /// </summary>
        /// <param name="order">Pending's order number to be deleted</param>
        /// <returns>DeletePending response task</returns>
        new public DeletePendingResponse DeletePending(long order)
        {
            return SyncCommandExecuteWrapper<DeletePendingResponse>(base.DeletePending(order));
        }

        /// <summary>
        /// Returns array of all symbols available for the user.
        /// </summary>
        /// <returns>GetAllSymbols response task</returns>
        new public GetAllSymbolsResponse GetAllSymbols()
        {
            return SyncCommandExecuteWrapper<GetAllSymbolsResponse>(base.GetAllSymbols());
        }

        /// <summary>
        /// Returns calendar with market events.
        /// </summary>
        /// <returns>GetCalendar response task</returns>
        new public GetCalendarResponse GetCalendar()
        {
            return SyncCommandExecuteWrapper<GetCalendarResponse>(base.GetCalendar());
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
        new public GetCandlesResponse GetCandles(PeriodCode period, string symbol, long start, long end = 0)
        {
            return SyncCommandExecuteWrapper<GetCandlesResponse>(base.GetCandles(period, symbol, start, end));
        }

        /// <summary>
        /// Returns candles data between given start and end dates. 
        /// If the chosen period is greater than 1 minute and the end date is not set, the last candle returned by the API can change 
        /// until the end of the period (the candle is being automatically updated every minute).
        /// </summary>
        /// <param name="period">Requested candles interval in minutes</param>
        /// <param name="symbol">Name of candle symbol</param>
        /// <param name="end">Defines the end timestamp of candles block (rounded down to the nearest interval)</param>
        /// <param name="number">Number of requested candles</param>
        /// <returns>GetCandles response task</returns>
        new public GetCandlesResponse GetNCandles(PeriodCode period, string symbol, long end, long number)
        {
            return SyncCommandExecuteWrapper<GetCandlesResponse>(base.GetNCandles(period, symbol, end, number));
        }

        /// <summary>
        /// Returns calculation of commission and rate of exchange. 
        /// The value is calculated as expected value and therefore might not be perfectly accurate.
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="volume">Volume</param>
        /// <returns>GetCommissionDef response task</returns>
        new public GetCommissionDefResponse GetCommissionDef(string symbol, double volume)
        {
            return SyncCommandExecuteWrapper<GetCommissionDefResponse>(base.GetCommissionDef(symbol, volume));
        }

        /// <summary>
        /// Returns information about account currency, and account leverage.
        /// </summary>
        /// <returns>GetCurrentUserData response task</returns>
        new public GetAccountInfoResponse GetAccountInfo()
        {
            return SyncCommandExecuteWrapper<GetAccountInfoResponse>(base.GetAccountInfo());
        }

        /// <summary>
        /// Returns IBs data from the given time range.
        /// </summary>
        /// <param name="start">Start of IBs history block</param>
        /// <param name="end">End of IBs history block</param>
        /// <returns>GetIbsHistory response task</returns>
        new public GetIbsHistoryResponse GetIbsHistory(long start, long end)
        {
            return SyncCommandExecuteWrapper<GetIbsHistoryResponse>(base.GetIbsHistory(start, end));
        }

        /// <summary>
        /// Returns margin level for account.
        /// </summary>
        /// <returns>GetMarginLevel response task</returns>
        new public GetAccountIndicatorsResponse GetAccountIndicators()
        {
            return SyncCommandExecuteWrapper<GetAccountIndicatorsResponse>(base.GetAccountIndicators());
        }

        /// <summary>
        /// Returns expected margin for given instrument and volume. The value is calculated as expected margin value, and therefore might not be perfectly accurate.
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="volume">Volume</param>
        /// <returns>GetMarginTrade response task</returns>
        new public GetMarginTradeResponse GetMarginTrade(string symbol, double volume)
        {
            return SyncCommandExecuteWrapper<GetMarginTradeResponse>(base.GetMarginTrade(symbol, volume));
        }

        /// <summary>
        /// Returns news from trading server which were sent within specified period of time.
        /// </summary>
        /// <param name="start">Time</param>
        /// <param name="end">Time, 0 means current time for simplicity</param>
        /// <returns>GetNews response task</returns>
        new public GetNewsResponse GetNews(long start, long end)
        {
            return SyncCommandExecuteWrapper<GetNewsResponse>(base.GetNews(start, end));
        }

        /// <summary>
        /// Returns current transaction status. At any time of transaction processing client might check the status of transaction on server side. In order to do that client must provide unique order taken from addOrder invocation.
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>GetOrderStatus response task</returns>
        new public GetOrderStatusResponse GetOrderStatus(long order)
        {
            return SyncCommandExecuteWrapper<GetOrderStatusResponse>(base.GetOrderStatus(order));
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
        new public GetProfitCalculationResponse GetProfitCalculation(string symbol, double volume, double openPrice, double closePrice, Side side, TradeType tradeType)
        {
            return SyncCommandExecuteWrapper<GetProfitCalculationResponse>(base.GetProfitCalculation(symbol, volume, openPrice, closePrice, side, tradeType));
        }

        /// <summary>
        /// Returns current time on trading server.
        /// </summary>
        /// <returns>GetServerTime response task</returns>
        new public GetServerTimeResponse GetServerTime()
        {
            return SyncCommandExecuteWrapper<GetServerTimeResponse>(base.GetServerTime());
        }

        /// <summary>
        /// Returns a list of step rules for DMAs.
        /// </summary>
        /// <returns>GetStepRules response task</returns>
        new public GetStepRulesResponse GetStepRules()
        {
            return SyncCommandExecuteWrapper<GetStepRulesResponse>(base.GetStepRules());
        }

        /// <summary>
        /// Gets information about symbol available for the user.
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <returns>GetSymbol response task</returns>
        new public GetSymbolResponse GetSymbol(string symbol)
        {
            return SyncCommandExecuteWrapper<GetSymbolResponse>(base.GetSymbol(symbol));
        }

        /// <summary>
        /// Returns a list of TICK_RECORDs  for the given symbols (only records that changed from the given timestamp are returned). New timestamp obtained from output will be used as an argument of the next call of this command.
        /// </summary>
        /// <param name="symbols">Symbols</param>
        /// <param name="level">Price level</param>
        /// <param name="timestamp">The time from which the most recent tick should be looked for. Historical prices cannot be obtained using this parameter. It can only be used to verify whether a price has changed since the given time</param>
        /// <returns>GetTickPrices response task</returns>
        new public GetTickPricesResponse GetTickPrices(IEnumerable<string> symbols, int level)
        {
            return SyncCommandExecuteWrapper<GetTickPricesResponse>(base.GetTickPrices(symbols, level));
        }

        /// <summary>
        /// Returns array of trades listed in orders argument.
        /// </summary>
        /// <param name="orders">Orders</param>
        /// <returns>GetTradeRecords response task</returns>
        new public GetTradeRecordsResponse GetTradeRecords(IEnumerable<int> orders)
        {
            return SyncCommandExecuteWrapper<GetTradeRecordsResponse>(base.GetTradeRecords(orders));
        }

        /// <summary>
        /// Returns array of user's trades.
        /// </summary>
        /// <returns>GetTrades response task</returns>
        new public GetTradesResponse GetTrades()
        {
            return SyncCommandExecuteWrapper<GetTradesResponse>(base.GetTrades());
        }

        /// <summary>
        /// Returns array of user's trades which were closed within specified period of time.
        /// </summary>
        /// <param name="start">Time, 0 means current time for simplicity</param>
        /// <param name="end">Time, 0 means last month interval</param>
        /// <returns>GetTradesHistory response task</returns>
        new public GetTradesHistoryResponse GetTradesHistory(long start, long end)
        {
            return SyncCommandExecuteWrapper<GetTradesHistoryResponse>(base.GetTradesHistory(start, end));
        }

        /// <summary>
        /// Returns quotes and trading times.
        /// </summary>
        /// <param name="symbols">Array of symbol names (Strings)</param>
        /// <returns>GetTradingHours response task</returns>
        new public GetTradingHoursResponse GetTradingHours(IEnumerable<string> symbols)
        {
            return SyncCommandExecuteWrapper<GetTradingHoursResponse>(base.GetTradingHours(symbols));
        }

        /// <summary>
        /// Returns the current API version.
        /// </summary>
        /// <returns>GetVersion response task</returns>
        new public GetVersionResponse GetVersion()
        {
            return SyncCommandExecuteWrapper<GetVersionResponse>(base.GetVersion());
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
        new public ModifyPendingResponse ModifyPending(long order, double price, double sl, double tp, long expiration, string customComment)
        {
            return SyncCommandExecuteWrapper<ModifyPendingResponse>(base.ModifyPending(order, price, sl, tp, expiration, customComment));
        }

        /// <summary>
        /// Modifies the given position.
        /// </summary>
        /// <param name="position">Number of position to be modified</param>
        /// <param name="sl">Stop loss value (or 0.0 if not set)</param>
        /// <param name="tp">Take profit value (or 0.0 if not set)</param>
        /// <param name="customComment">The value the customer may provide in order to retrieve it later</param>
        /// <returns>ModifyPosition response task</returns>
        new public ModifyPositionResponse ModifyPosition(long position, double sl, double tp)
        {
            return SyncCommandExecuteWrapper<ModifyPositionResponse>(base.ModifyPosition(position, sl, tp));
        }

        /// <summary>
        /// Regularly calling this function is enough to refresh the internal state of all the components in the system. It is recommended that any application that does not execute other commands, should call this command at least once every 10 minutes. Please note that the streaming counterpart of this function is called getKeepAlive.
        /// </summary>
        /// <returns>Ping response task</returns>
        new public PingResponse Ping()
        {
            return SyncCommandExecuteWrapper<PingResponse>(base.Ping());
        }
    }
}
