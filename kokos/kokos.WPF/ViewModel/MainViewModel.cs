﻿using System;
using System.Reactive.Linq;
using kokos.WPF.ServerConnect;
using kokos.WPF.ViewModel.Base;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;

namespace kokos.WPF.ViewModel
{
    public class MainViewModel : AReactiveViewModel
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

            this.ObservableForProperty(x => x.SelectedSymbol)
                .Throttle(TimeSpan.FromMilliseconds(200))
                .InvokeCommand(this, x => x.SelectedSymbol.LoadTickData);
        }

        private void PopulateSymbols()
        {
            Symbols.Clear();

            foreach (var symbol in SyncApiWrapper.Instance.SymbolRecords)
            {
                Symbols.Add(new SymbolViewModel {Name = symbol.Symbol, Bid = symbol.Bid, Ask = symbol.Ask});
            }

            SelectedSymbol = Symbols.FirstOrDefault();
        }
    }
}
