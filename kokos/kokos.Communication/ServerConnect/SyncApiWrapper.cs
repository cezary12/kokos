using kokos.Abstractions;
using kokos.Communication.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using xAPI;
using xAPI.Codes;
using xAPI.Connection;
using xAPI.Responses;
using xAPI.Utils;

namespace kokos.Communication.ServerConnect
{
    public class SyncApiWrapper
    {
        private static readonly Server ServerDemo = Servers.DEMO;
#warning no Servers.REAL
        private static readonly Server ServerReal = Servers.DEMO;

        private const string AppId = "testid";
        private const string AppName = "testname";

        private APISync _connector;

        public List<Symbol> SymbolRecords { get; private set; } 

        private string _userId;
        private SecureString _password;
        private bool _isDemo;

        public void Login(string userId, SecureString password, bool isDemo)
        {
            _userId = userId;
            _password = password;
            _isDemo = isDemo;

            var loginResponse = ConnectToServer();
            ThrowIfNotSuccessful(loginResponse);

            var allSymbolsResponse = _connector.GetAllSymbols();
            ThrowIfNotSuccessful(allSymbolsResponse);

            SymbolRecords = allSymbolsResponse.SymbolRecords.Select(x => new Symbol
            {
                Name = x.Symbol,
                CategoryName = x.CategoryName,
                Description = x.Description,
                Ask = x.Ask,
                Bid = x.Bid
            }).ToList();

            //var symbolResponse = APICommandFactory.ExecuteSymbolCommand(_connector, "EURUSD", true);
            //ThrowIfNotSuccessful(symbolResponse);
        }

        private LoginResponse ConnectToServer()
        {
            if (_connector != null)
                _connector.Logout();

            _connector = new APISync(_isDemo ? ServerDemo : ServerReal);
            var credentials = new Credentials(_userId, _password.ToInsecureString(), AppId, AppName);
            var loginResponse = _connector.Login(credentials);

            return loginResponse;
        }

        public void Logout()
        {
            var logoutResponse = _connector.Logout();
            ThrowIfNotSuccessful(logoutResponse);
        }

        public List<TickData> LoadData(string symbol, DataPeriod dataPeriod, DateTime startDate, DateTime endDate, long? ticks = null)
        {
            return TryExecute(() =>
            {
                var start = startDate.ToUnixMilliseconds();
                var end = endDate.ToUnixMilliseconds();
                var periodCode = MapToPeriodCode(dataPeriod);
                var range = _connector.GetCandles(periodCode, symbol, start, end);

                ThrowIfNotSuccessful(range);

                var tickData = range.Candles.Convert().ToList();

                return tickData;
            });
        }

        private static PeriodCode MapToPeriodCode(DataPeriod dataPeriod)
        {
            switch (dataPeriod)
            {
                case DataPeriod.D1: return PeriodCode.Days(1);
                case DataPeriod.M5: return PeriodCode.Minutes(5);
                case DataPeriod.H1: return PeriodCode.Hours(1);
                default: 
                    throw new NotSupportedException("Not supported period type: " + dataPeriod);
            }
        }

        private T TryExecute<T>(Func<T> execute, [CallerMemberName] string callerMemberName = null)
        {
            var retry = 0;
            Exception lastException;
            do
            {
                try
                {
                    return execute();
                }
                catch (Exception ex)
                {
                    lastException = ex;

                    var loginResponse = ConnectToServer();
                    ThrowIfNotSuccessful(loginResponse);
                }

            } while (++retry <= 3);

            throw new Exception("Unable to execute function " + callerMemberName, lastException);
        }

        private static void ThrowIfNotSuccessful(APIResponse response, [CallerMemberName] string callerMemberName = null)
        {
            if (response.Status != true)
                throw new Exception("Error while executing " + callerMemberName);
        }
    }
}
