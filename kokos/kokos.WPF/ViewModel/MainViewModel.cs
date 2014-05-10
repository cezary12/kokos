using System.Linq;
using kokos.WPF.ServerConnect;
using kokos.WPF.ViewModel.Base;
using System.Collections.ObjectModel;

namespace kokos.WPF.ViewModel
{
    public class MainViewModel : AViewModel
    {
        public LoginViewModel LoginViewModel { get; private set; }

        public ObservableCollection<SymbolViewModel> Symbols { get; private set; }

        public SymbolViewModel SelectedSymbol
        {
            get { return GetValue<SymbolViewModel>(); }
            set { SetValue(value); }
        }

        public MainViewModel()
        {
            LoginViewModel = new LoginViewModel(PopulateSymbols);
            Symbols = new ObservableCollection<SymbolViewModel>();
        }

        private void PopulateSymbols()
        {
            Symbols.Clear();

            foreach (var symbol in SyncApiWrapper.Instance.SymbolRecords)
            {
                Symbols.Add(new SymbolViewModel { Name = symbol.Symbol, Bid = symbol.Bid, Ask = symbol.Ask });
            }

            SelectedSymbol = Symbols.FirstOrDefault();
        }
    }
}
