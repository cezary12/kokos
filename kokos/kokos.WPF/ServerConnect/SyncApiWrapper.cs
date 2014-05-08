using System.Security;
using kokos.WPF.Security;
using kokos.WPF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using xAPI.Codes;
using xAPI.Commands;
using xAPI.Records;
using xAPI.Responses;
using xAPI.Sync;

namespace kokos.WPF.ServerConnect
{
    public class SyncApiWrapper
    {
        private static readonly Server Server = Servers.DEMO;
        private static readonly SyncAPIConnector Connector = new SyncAPIConnector(Server);

        public List<SymbolRecord> SymbolRecords { get; private set; } 

        public static readonly SyncApiWrapper Instance = new SyncApiWrapper();

        private string _userId;
        private SecureString _password;

        private SyncApiWrapper()
        {
        }

        public void Login(string userId, SecureString password)
        {
            var credentials = new Credentials(userId, password.ToInsecureString(), "", "kokos");
            var loginResponse = APICommandFactory.ExecuteLoginCommand(Connector, credentials, true);
            ThrowIfNotSuccessful(loginResponse);

            var allSymbolsResponse = APICommandFactory.ExecuteAllSymbolsCommand(Connector, true);
            ThrowIfNotSuccessful(allSymbolsResponse);

            _userId = userId;
            _password = password;

            SymbolRecords = new List<SymbolRecord>(allSymbolsResponse.SymbolRecords);

            var symbolResponse = APICommandFactory.ExecuteSymbolCommand(Connector, "EURUSD", true);
            ThrowIfNotSuccessful(symbolResponse);
        }

        public void Logout()
        {
            var logoutResponse = APICommandFactory.ExecuteLogoutCommand(Connector);
            ThrowIfNotSuccessful(logoutResponse);
        }

        public List<TickData> LoadData(string symbol, PERIOD_CODE periodCode, DateTime? startDate = null, DateTime? endDate = null, long? ticks = null)
        {
            var start = startDate == null ? (long?)null : startDate.Value.ToUnixMilliseconds();
            var end = endDate == null ? (long?)null : endDate.Value.ToUnixMilliseconds();

            var chartRangeInfoRecord = new ChartRangeInfoRecord(symbol, periodCode, start, end, ticks);
            var range = APICommandFactory.ExecuteChartRangeCommand(Connector, chartRangeInfoRecord, true);
            
            ThrowIfNotSuccessful(range);

            var tickData = range.RateInfos.Select(x => new TickData(x, range.Digits ?? 1)).ToList();

            return tickData;
        }

        private void ThrowIfNotSuccessful(BaseResponse response, [CallerMemberName] string callerMemberName = null)
        {
            if (response.Status != true)
                throw new Exception("Error while executing " + callerMemberName);
        }
    }
}
