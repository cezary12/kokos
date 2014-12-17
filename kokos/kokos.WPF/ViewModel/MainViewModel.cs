using System.Security;
using kokos.Communication.Extensions;
using kokos.Communication.ServerConnect;
using kokos.WPF.Properties;
using kokos.WPF.ViewModel.Base;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace kokos.WPF.ViewModel
{
    public class MainViewModel : AReactiveViewModel
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

        private List<SymbolViewModel> _symbols;
        public ObservableCollection<SymbolViewModel> Symbols { get; private set; }

        public IReactiveCommand SearchSymbolCommand { get; private set; }

        public string SearchText
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public SymbolViewModel SelectedSymbol
        {
            get { return GetValue<SymbolViewModel>(); }
            set { SetValue(value); }
        }

        public MainViewModel(XtbWrapper xtbWrapper)
        {
            RxApp.MainThreadScheduler = new DispatcherScheduler(Application.Current.Dispatcher);

            _xtbWrapper = xtbWrapper;

            Symbols = new ObservableCollection<SymbolViewModel>();

            SearchSymbolCommand = ReactiveCommand.CreateAsyncTask(ExecuteSearchSymbol, RxApp.MainThreadScheduler);

            this.ObservableForProperty(x => x.SelectedSymbol)
                .Throttle(TimeSpan.FromSeconds(0.1), RxApp.MainThreadScheduler)
                .Subscribe(ExecuteLoadTickData);

            this.ObservableForProperty(x => x.SearchText)
                .Throttle(TimeSpan.FromSeconds(0.2), RxApp.MainThreadScheduler)
                .Subscribe(SearchSymbolCommand.Execute);

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

        private void ExecuteLoadTickData(object parameter)
        {
            if (SelectedSymbol == null)
                return;

            SelectedSymbol.LoadTickData.Execute(null);
        }

        private async Task<bool> ExecuteSearchSymbol(object parameter)
        {
            var symbols = await Task.Run(() => Filter());
            AddSymbolsToCollection(symbols);

            return true;
        }

        private IEnumerable<SymbolViewModel> Filter()
        {
            if (string.IsNullOrEmpty(SearchText))
                return _symbols;

            return _symbols.Where(x => x.Name.StartsWith(SearchText, StringComparison.InvariantCultureIgnoreCase))
                .Union(_symbols.Where(x => ContainsSearchText(x.Name)))
                .Union(_symbols.Where(x => ContainsSearchText(x.CategoryName)))
                .Union(_symbols.Where(x => ContainsSearchText(x.Description)));
        }

        private bool ContainsSearchText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            return text.IndexOf(SearchText, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        private void AddSymbolsToCollection(IEnumerable<SymbolViewModel> symbols)
        {
            Symbols.Clear();
            foreach (var symbol in symbols)
                Symbols.Add(symbol);

            SelectedSymbol = Symbols.FirstOrDefault();
        }

        private IObservable<bool> CreateCanExecutePasswordObservable()
        {
            return this.WhenAny(x => x.Login, x => x.Password, CanExecuteLoginCommand);
        }

        private static bool CanExecuteLoginCommand(IObservedChange<MainViewModel, string> login, IObservedChange<MainViewModel, SecureString> password)
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

                _symbols = (await _xtbWrapper.LoadSymbols())
                    .Select(symbol => new SymbolViewModel(_xtbWrapper, symbol.Name, symbol.Description)
                    {
                        CategoryName = symbol.CategoryName,

                        Bid = symbol.Bid,
                        Ask = symbol.Ask
                    })
                    .ToList();

                AddSymbolsToCollection(_symbols);

                foreach (var s in _symbols)
                {
// ReSharper disable once CSharpWarnings::CS4014
                    s.UpdatePreviewPlot();
                }

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
