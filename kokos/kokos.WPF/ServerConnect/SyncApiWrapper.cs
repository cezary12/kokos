using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

        private SyncApiWrapper()
        {
        }

        public void Login(string userId, string password)
        {
            var credentials = new Credentials(userId, password, "", "kokos");
            var loginResponse = APICommandFactory.ExecuteLoginCommand(Connector, credentials, true);
            ThrowIfNotSuccessful(loginResponse);

            var allSymbolsResponse = APICommandFactory.ExecuteAllSymbolsCommand(Connector, true);
            ThrowIfNotSuccessful(allSymbolsResponse);
            SymbolRecords = new List<SymbolRecord>(allSymbolsResponse.SymbolRecords);

            var symbolResponse = APICommandFactory.ExecuteSymbolCommand(Connector, "EURUSD", true);
            ThrowIfNotSuccessful(symbolResponse);
        }

        public void Logout()
        {
            var logoutResponse = APICommandFactory.ExecuteLogoutCommand(Connector);
            ThrowIfNotSuccessful(logoutResponse);
        }

        private void ThrowIfNotSuccessful(BaseResponse response, [CallerMemberName] string callerMemberName = null)
        {
            if (response.Status != true)
                throw new Exception("Error while executing " + callerMemberName);
        }
    }
}
