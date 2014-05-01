using kokos.WPF.Properties;
using kokos.WPF.Security;
using kokos.WPF.ServerConnect;
using kokos.WPF.ViewModel.Base;
using System.Threading.Tasks;

namespace kokos.WPF.ViewModel
{
    public class LoginViewModel : AViewModel
    {
        public string Login
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Password
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public bool RememberLoginData
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool IsBusy
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool IsLoggedIn
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public AsyncRelayCommand LoginCommand { get; private set; }
        public AsyncRelayCommand LogoutCommand { get; private set; }

        private readonly SyncApiWrapper _syncWrapper = new SyncApiWrapper();

        public LoginViewModel()
        {
            LoginCommand = new AsyncRelayCommand(ExecuteLoginAsync, param => !IsLoggedIn);
            LogoutCommand = new AsyncRelayCommand(ExecuteLogoutAsync, param => IsLoggedIn);

            RememberLoginData = Settings.Default.RememberLoginData;
            Login = Settings.Default.Login;
            Password = Settings.Default.Password.Decrypt();
        }

        private async Task ExecuteLoginAsync(object parameter)
        {
            IsBusy = true;

            await Task
                .Run(() => _syncWrapper.Login(Login, Password))
                .ContinueWith(x =>
                {
                    Settings.Default.RememberLoginData = RememberLoginData;
                    Settings.Default.Login = RememberLoginData ? Login : "";
                    Settings.Default.Password = RememberLoginData ? Password.Encrypt() : "";
                    Settings.Default.Save();
                });

            IsBusy = false;
            IsLoggedIn = true;
        }

        private async Task ExecuteLogoutAsync(object parameter)
        {
            IsLoggedIn = false;
            IsBusy = true;
            await Task.Run(() => _syncWrapper.Logout());
            IsBusy = false;
        }
    }
}
