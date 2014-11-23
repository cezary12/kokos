using kokos.Communication.ServerConnect;
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
        public LoginViewModel LoginViewModel { get; private set; }

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

        public MainViewModel(LoginViewModel loginViewModel)
        {
            RxApp.MainThreadScheduler = new DispatcherScheduler(Application.Current.Dispatcher);

            LoginViewModel = loginViewModel;
            Symbols = new ObservableCollection<SymbolViewModel>();

            LoginViewModel.ObservableForProperty(x => x.IsLoggedIn).Subscribe(x => PopulateSymbols());

            SearchSymbolCommand = ReactiveCommand.CreateAsyncTask(ExecuteSearchSymbol, RxApp.MainThreadScheduler);

            this.ObservableForProperty(x => x.SelectedSymbol)
                .Throttle(TimeSpan.FromSeconds(0.1), RxApp.MainThreadScheduler)
                .Subscribe(ExecuteLoadTickData);

            this.ObservableForProperty(x => x.SearchText)
                .Throttle(TimeSpan.FromSeconds(0.2), RxApp.MainThreadScheduler)
                .Subscribe(SearchSymbolCommand.Execute);
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

        private void PopulateSymbols()
        {
            _symbols = SyncApiWrapper.Instance.SymbolRecords
                .Select(symbol => new SymbolViewModel
                {
                    Name = symbol.Name,
                    CategoryName = symbol.CategoryName,
                    Description = symbol.Description,

                    Bid = symbol.Bid,
                    Ask = symbol.Ask
                })
                .ToList();

            AddSymbolsToCollection(_symbols);
        }

        private void AddSymbolsToCollection(IEnumerable<SymbolViewModel> symbols)
        {
            Symbols.Clear();
            foreach (var symbol in symbols)
                Symbols.Add(symbol);

            SelectedSymbol = Symbols.FirstOrDefault();
        }
    }
}
