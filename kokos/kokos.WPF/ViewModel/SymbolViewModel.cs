using kokos.WPF.ServerConnect;
using kokos.WPF.ViewModel.Base;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using xAPI.Codes;

namespace kokos.WPF.ViewModel
{
    public class SymbolViewModel : AViewModel
    {
        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public double? Bid
        {
            get { return GetValue<double?>(); }
            set { SetValue(value); }
        }

        public double? Ask
        {
            get { return GetValue<double?>(); }
            set { SetValue(value); }
        }

        public bool IsBusy
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public ObservableCollection<TickData> Ticks { get; private set; } 

        public AsyncRelayCommand LoadTickData { get; private set; }


        public SymbolViewModel()
        {
            Ticks = new ObservableCollection<TickData>();
            LoadTickData = new AsyncRelayCommand(ExecuteLoadTickDataAsync, x => !IsBusy);
        }

        private async Task ExecuteLoadTickDataAsync(object parameter)
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
        }
    }
}
