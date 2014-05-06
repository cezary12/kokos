using kokos.WPF.ServerConnect;
using kokos.WPF.ViewModel.Base;
using System.Collections.ObjectModel;

namespace kokos.WPF.ViewModel
{
    public class MainViewModel : AViewModel
    {
        private readonly SyncApiWrapper _syncWrapper = new SyncApiWrapper();

        public LoginViewModel LoginViewModel { get; private set; }

        public ObservableCollection<SymbolViewModel> Symbols { get; private set; }

        public MainViewModel()
        {
            LoginViewModel = new LoginViewModel(_syncWrapper, PopulateSymbols);
            Symbols = new ObservableCollection<SymbolViewModel>();
        }

        private void PopulateSymbols()
        {
            Symbols.Clear();

            foreach (var symbol in _syncWrapper.SymbolRecords)
            {
                Symbols.Add(new SymbolViewModel { Name = symbol.Symbol, Bid = symbol.Bid, Ask = symbol.Ask });
            }
        }
    }
}
