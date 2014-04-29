using System.Threading.Tasks;
using kokos.WPF.ViewModel.Base;
using System.Threading;

namespace kokos.WPF.ViewModel
{
    public class LoginViewModel : AViewModel
    {
        public string LoginName
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

        public LoginViewModel()
        {
            LoginCommand = new AsyncRelayCommand(ExecuteLoginAsync, param => !IsLoggedIn);
            LogoutCommand = new AsyncRelayCommand(ExecuteLogoutAsync, param => IsLoggedIn);
        }

        private async Task ExecuteLoginAsync(object parameter)
        {
            IsBusy = true;
            await Task.Delay(2000);
            IsBusy = false;
            IsLoggedIn = true;
        }

        private async Task ExecuteLogoutAsync(object parameter)
        {
            IsLoggedIn = false;
            IsBusy = true;
            await Task.Delay(20);
            IsBusy = false;
        }
    }
}
