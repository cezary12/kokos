using kokos.WPF.Properties;
using kokos.WPF.Security;
using kokos.WPF.ServerConnect;
using kokos.WPF.ViewModel.Base;
using ReactiveUI;
using System;
using System.Security;
using System.Threading.Tasks;

namespace kokos.WPF.ViewModel
{
    public class LoginViewModel : ReactiveObject
    {
        private readonly Action _executeOnLogging;

        private string _login;
        public string Login
        {
            get { return _login; }
            set { this.RaiseAndSetIfChanged(ref _login, value); }
        }

        private SecureString _password;
        public SecureString Password
        {
            get { return _password; }
            set { this.RaiseAndSetIfChanged(ref _password, value); }
        }

        private bool _rememberLoginData;
        public bool RememberLoginData
        {
            get { return _rememberLoginData; }
            set { this.RaiseAndSetIfChanged(ref _rememberLoginData, value); }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { this.RaiseAndSetIfChanged(ref _isBusy, value); }
        }

        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { this.RaiseAndSetIfChanged(ref _isLoggedIn, value); }
        }

        public IReactiveCommand LoginCommand { get; private set; }
        public IReactiveCommand LogoutCommand { get; private set; }

        public LoginViewModel(Action executeOnLogging)
        {
            if (this.IsInDesignMode())
            {
                IsLoggedIn = true;
            }

            _executeOnLogging = executeOnLogging;

            LoginCommand = ReactiveCommand.CreateAsync(CreateCanExecutePasswordObservable(), ExecuteLoginAsync, RxApp.MainThreadScheduler);
            LogoutCommand = ReactiveCommand.CreateAsync(ExecuteLogoutAsync, RxApp.MainThreadScheduler);

            RememberLoginData = Settings.Default.RememberLoginData;
            Login = Settings.Default.Login;
            Password = Settings.Default.Password.Decrypt().ToSecureString();
        }

        private IObservable<bool> CreateCanExecutePasswordObservable()
        {
            return this.WhenAny(x => x.Login, x => x.Password, CanExecuteLoginCommand);
        }

        private static bool CanExecuteLoginCommand(IObservedChange<LoginViewModel, string> login, IObservedChange<LoginViewModel, SecureString> password)
        {
            return !string.IsNullOrEmpty(login.Value) && !string.IsNullOrEmpty(password.Value.ToInsecureString());
        }

        private async Task<bool> ExecuteLoginAsync(object parameter)
        {
            IsBusy = true;

            await Task
                .Run(() => SyncApiWrapper.Instance.Login(Login, Password))
                .ContinueWith(x =>
                {
                    Settings.Default.RememberLoginData = RememberLoginData;
                    Settings.Default.Login = RememberLoginData ? Login : "";
                    Settings.Default.Password = RememberLoginData ? Password.ToInsecureString().Encrypt() : "";
                    Settings.Default.Save();
                });

            _executeOnLogging();

            IsBusy = false;
            IsLoggedIn = true;

            return true;
        }

        private async Task<bool> ExecuteLogoutAsync(object parameter)
        {
            IsLoggedIn = false;
            IsBusy = true;
            await Task.Run(() => SyncApiWrapper.Instance.Logout());
            IsBusy = false;

            return true;
        }
    }
}
