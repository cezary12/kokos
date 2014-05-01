using System;
using System.Runtime.CompilerServices;
using xAPI.Commands;
using xAPI.Responses;
using xAPI.Sync;

namespace kokos.WPF.ServerConnect
{
    public class SyncApiWrapper
    {
        private static readonly Server Server = Servers.DEMO;
        private readonly SyncAPIConnector _connector;

        public SyncApiWrapper()
        {
            _connector = new SyncAPIConnector(Server);
        }

        public void Login(string userId, string password)
        {
            var credentials = new Credentials(userId, password, "", "kokos");
            var loginResponse = APICommandFactory.ExecuteLoginCommand(_connector, credentials, true);
            ThrowIfNotSuccessful(loginResponse);
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
