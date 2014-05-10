using kokos.WPF.ServerConnect;
using kokos.WPF.ViewModel.Base;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using xAPI.Codes;

namespace kokos.WPF.ViewModel
{
    public class SymbolViewModel : ReactiveObject
    {

        private string _name;
        public string Name
        {
            get { return _name; }
            set { this.RaiseAndSetIfChanged(ref _name, value); }
        }

        private double? _bid;
        public double? Bid
        {
            get { return _bid; }
            set { this.RaiseAndSetIfChanged(ref _bid, value); }
        }


        private double? _ask;
        public double? Ask
        {
            get { return _ask; }
            set { this.RaiseAndSetIfChanged(ref _ask, value); }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { this.RaiseAndSetIfChanged(ref _isBusy, value); }
        }

        public ObservableCollection<TickData> Ticks { get; private set; } 

        public IReactiveCommand LoadTickData { get; private set; }


        public SymbolViewModel()
        {
            if (this.IsInDesignMode())
                IsBusy = true;

            Ticks = new ObservableCollection<TickData>();
            LoadTickData = ReactiveCommand.CreateAsync(this.WhenAny(x => x.IsBusy, x => !x.Value), ExecuteLoadTickDataAsync, RxApp.MainThreadScheduler);
        }

        private async Task<bool> ExecuteLoadTickDataAsync(object parameter)
        {
            IsBusy = true;

            var period = PERIOD_CODE.PERIOD_M5;

            var endDate = DateTime.Now;

            var startDate = endDate.AddMonths(-3);
            startDate = new DateTime(startDate.Year, startDate.Month, 1);

            var tickCount = 1000000;

            var ticks = await Task.Run(() => SyncApiWrapper.Instance.LoadData(Name, period, startDate, endDate, tickCount));

            Ticks.Clear();
            foreach (var tick in ticks)
                Ticks.Add(tick);

            IsBusy = false;

            return true;
        }
    }
}
