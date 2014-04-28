using kokos.WPF.ViewModel.Base;

namespace kokos.WPF.ViewModel
{
    public class MainViewModel : AViewModel
    {
        private readonly LoginViewModel _loginViewModel = new LoginViewModel();

        public LoginViewModel LoginViewModel
        {
            get { return _loginViewModel; }
        }
    }
}
