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
        private readonly SyncAPIConnector _connector;

        public List<SymbolRecord> SymbolRecords { get; private set; } 

        public SyncApiWrapper()
        {
            _connector = new SyncAPIConnector(Server);
        }

        public void Login(string userId, string password)
        {
            var credentials = new Credentials(userId, password, "", "kokos");
            var loginResponse = APICommandFactory.ExecuteLoginCommand(_connector, credentials, true);
            ThrowIfNotSuccessful(loginResponse);

            var allSymbolsResponse = APICommandFactory.ExecuteAllSymbolsCommand(_connector, true);
            ThrowIfNotSuccessful(allSymbolsResponse);
            SymbolRecords = new List<SymbolRecord>(allSymbolsResponse.SymbolRecords);

            var symbolResponse = APICommandFactory.ExecuteSymbolCommand(_connector, "EURUSD", true);
            ThrowIfNotSuccessful(symbolResponse);
        }

        public void Logout()
        {
            var logoutResponse = APICommandFactory.ExecuteLogoutCommand(_connector);
            ThrowIfNotSuccessful(logoutResponse);
        }

        private void ThrowIfNotSuccessful(BaseResponse response, [CallerMemberName] string callerMemberName = null)
        {
            if (response.Status != true)
                throw new Exception("Error while executing " + callerMemberName);
        }
    }
}
