using kokos.Abstractions;
using kokos.Communication.Extensions;
using kokos.Communication.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Threading.Tasks;
using xAPI;
using xAPI.Codes;
using xAPI.Connection;
using xAPI.Responses;
using xAPI.Utils;

namespace kokos.Communication.ServerConnect
{
    public class XtbWrapper
    {
        private static readonly Server ServerDemo = Servers.DEMO;
#warning no Servers.REAL
        private static readonly Server ServerReal = Servers.DEMO;

        private const string AppId = "testid";
        private const string AppName = "testname";

        private API _connector;

        public List<Symbol> SymbolRecords { get; private set; } 

        private string _userId;
        private SecureString _password;
        private bool _isDemo;

        private readonly SymbolDb _symbolDb;

        public XtbWrapper(SymbolDb symbolDb)
        {
            _symbolDb = symbolDb;
        }

        public async Task<bool> Login(string userId, SecureString password, bool isDemo)
        {
            _userId = userId;
            _password = password;
            _isDemo = isDemo;

            var loginResponse = await ConnectToServer();
            ThrowIfNotSuccessful(loginResponse);

            if (_symbolDb.Symbols.Count > 0)
            {
                SymbolRecords = _symbolDb
                    .Symbols
                    .Select(x => x.Symbol)
                    .ToList();

                return true;
            }

            var allSymbolsResponse = await _connector.GetAllSymbols();
            ThrowIfNotSuccessful(allSymbolsResponse);

            SymbolRecords = allSymbolsResponse.SymbolRecords.Select(x => new Symbol
            {
                Name = x.Symbol,
                CategoryName = x.CategoryName,
                Description = x.Description,
                Ask = x.Ask,
                Bid = x.Bid
            }).ToList();

            foreach (var s in SymbolRecords)
            {
                try
                {
                    var daily = await LoadData(s.Name, DurationEnum.Year10);
                    var intraday = await LoadData(s.Name, DurationEnum.Day1);

                    var ss = new SymbolStore
                    {
                        Symbol = s,
                        DailyTickData = daily,
                        IntradayTickData = intraday
                    };

                    _symbolDb.Symbols.Add(ss);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error while loading " + s.Name);
                }

            }

            return true;

            //var symbolResponse = APICommandFactory.ExecuteSymbolCommand(_connector, "EURUSD", true);
            //ThrowIfNotSuccessful(symbolResponse);
        }

        public async Task<List<Symbol>> LoadSymbols()
        {
            var allSymbolsResponse = await _connector.GetAllSymbols();
            ThrowIfNotSuccessful(allSymbolsResponse);

            var symbolRecords = allSymbolsResponse.SymbolRecords.Select(x => new Symbol
            {
                Name = x.Symbol,
                CategoryName = x.CategoryName,
                Description = x.Description,
                Ask = x.Ask,
                Bid = x.Bid
            }).ToList();

            return symbolRecords;
        }

        private async Task<LoginResponse> ConnectToServer()
        {
            if (_connector != null)
                await _connector.Logout();

            _connector = new APISync(_isDemo ? ServerDemo : ServerReal);
            var credentials = new Credentials(_userId, _password.ToInsecureString(), AppId, AppName);
            var loginResponse =  await _connector.Login(credentials);

            return loginResponse;
        }

        public async Task<bool> Logout()
        {
            var logoutResponse = await _connector.Logout();
            ThrowIfNotSuccessful(logoutResponse);
            return true;
        }

        public async Task<List<TickData>> LoadData(string symbol, DurationEnum dataPeriod)
        {
            DateTime startDate, endDate;
            GetPeriodData(dataPeriod, out startDate, out endDate);

            var start = startDate.ToUnixMilliseconds();
            var end = endDate.ToUnixMilliseconds();
            var periodCode = MapToPeriodCode(dataPeriod);
            var range = await _connector.GetCandles(periodCode, symbol, start, end);

            ThrowIfNotSuccessful(range);

            var tickData = range.Candles.Convert().ToList();

            return tickData;
        }

        private static PeriodCode MapToPeriodCode(DurationEnum dataPeriod)
        {
            return dataPeriod == DurationEnum.Day1 
                ? PeriodCode.Minutes(5) 
                : PeriodCode.Days(1);
        }

        private async Task<T> TryExecute<T>(Func<Task<T>> execute, [CallerMemberName] string callerMemberName = null)
        {
            var retry = 0;
            Exception lastException;
            do
            {
                try
                {
                    return await execute();
                }
                catch (Exception ex)
                {
                    lastException = ex;
                }

                var loginResponse = await ConnectToServer();
                ThrowIfNotSuccessful(loginResponse);

            } while (++retry <= 3);

            throw new Exception("Unable to execute function " + callerMemberName, lastException);
        }

        private static void GetPeriodData(DurationEnum periodCode, out DateTime startDate, out DateTime endDate)
        {
            endDate = DateTime.Now;
            startDate = endDate.Date.AddYears(-1);
            switch (periodCode)
            {
                    case DurationEnum.Day1:
                    startDate = endDate.Date;
                    break;
                    case DurationEnum.Week1:
                    startDate = endDate.Date.AddDays(-7);

                    break;
            }

        }

        private static void ThrowIfNotSuccessful(APIResponse response, [CallerMemberName] string callerMemberName = null)
        {
            if (response.Status != true)
                throw new Exception("Error while executing " + callerMemberName);
        }
    }
}
