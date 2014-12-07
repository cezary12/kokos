using kokos.Communication.Extensions;
using kokos.Communication.ServerConnect;
using kokos.WPF.Properties;
using kokos.WPF.ViewModel.Base;
using ReactiveUI;
using System;
using System.Security;
using System.Threading.Tasks;

namespace kokos.WPF.ViewModel
{
    public class LoginViewModel : AReactiveViewModel
    {
        private readonly XtbWrapper _xtbWrapper;
        public string Login
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public SecureString Password
        {
            get { return GetValue<SecureString>(); }
            set { SetValue(value); }
        }

        public bool RememberLoginData
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool IsDemoAccount
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

        public IReactiveCommand LoginCommand { get; private set; }
        public IReactiveCommand LogoutCommand { get; private set; }

        public LoginViewModel(XtbWrapper xtbWrapper)
        {
            _xtbWrapper = xtbWrapper;

            if (IsInDesignMode)
            {
                IsLoggedIn = true;
            }

            LoginCommand = ReactiveCommand.CreateAsyncTask(CreateCanExecutePasswordObservable(), ExecuteLoginAsync, RxApp.MainThreadScheduler);
            LogoutCommand = ReactiveCommand.CreateAsyncTask(ExecuteLogoutAsync, RxApp.MainThreadScheduler);

            RememberLoginData = Settings.Default.RememberLoginData;
            Login = Settings.Default.Login;
            Password = Settings.Default.Password.Decrypt().ToSecureString();
            IsDemoAccount = Settings.Default.IsDemoAccount;
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

            try
            {
                await _xtbWrapper.Login(Login, Password, IsDemoAccount);

                Settings.Default.RememberLoginData = RememberLoginData;
                Settings.Default.Login = RememberLoginData ? Login : "";
                Settings.Default.Password = RememberLoginData ? Password.ToInsecureString().Encrypt() : "";
                Settings.Default.IsDemoAccount = RememberLoginData && IsDemoAccount;

                Settings.Default.Save();

                IsBusy = false;
                IsLoggedIn = true;
            }
            finally
            {
                IsBusy = false;
            }

            return true;
        }

        private async Task<bool> ExecuteLogoutAsync(object parameter)
        {
            IsLoggedIn = false;
            IsBusy = true;
            
            await _xtbWrapper.Logout();

            IsBusy = false;

            return true;
        }
    }
}
