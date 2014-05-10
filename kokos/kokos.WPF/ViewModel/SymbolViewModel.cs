using kokos.WPF.ServerConnect;
using kokos.WPF.ViewModel.Base;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
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

        private bool _isLoaded;
        public bool IsLoaded
        {
            get { return _isLoaded; }
            set { this.RaiseAndSetIfChanged(ref _isLoaded, value); }
        }

        private PlotModel _plot;
        public PlotModel Plot
        {
            get { return _plot; }
            set { this.RaiseAndSetIfChanged(ref _plot, value); }
        }

        public ObservableCollection<TickData> Ticks { get; private set; } 

        public IReactiveCommand LoadTickData { get; private set; }

        public SymbolViewModel()
        {
            if (this.IsInDesignMode())
                IsBusy = true;

            Ticks = new ObservableCollection<TickData>();
            LoadTickData = ReactiveCommand.CreateAsync(this.WhenAny(x => x.IsBusy, x => !x.Value), ExecuteLoadTickDataAsync, RxApp.MainThreadScheduler);
            Plot = CreateCandleStickSeries();
        }

        private async Task<bool> ExecuteLoadTickDataAsync(object parameter)
        {
            IsBusy = true;

            var period = PERIOD_CODE.PERIOD_D1;

            var endDate = DateTime.Now;

            var startDate = endDate.AddMonths(-3);
            startDate = new DateTime(startDate.Year, startDate.Month, 1);

            var tickCount = 1000000;

            var ticks = await Task.Run(() => SyncApiWrapper.Instance.LoadData(Name, period, startDate, endDate, tickCount));

            Ticks.Clear();
            foreach (var tick in ticks)
                Ticks.Add(tick);

            Plot = CreateCandleStickSeries();
            IsBusy = false;
            IsLoaded = true;

            return true;
        }

        private PlotModel CreateCandleStickSeries()
        {
            var pm = new PlotModel { Title = Name, LegendSymbolLength = 24 };

            var timeSpanAxis1 = new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "MM/dd/yyyy" };
            pm.Axes.Add(timeSpanAxis1);
            var linearAxis1 = new LinearAxis { Position = AxisPosition.Left, StringFormat = "N4"};
            pm.Axes.Add(linearAxis1);
            var candleStickSeries = new CandleStickSeries
            {
                //CandleWidth = 6,
                Title = Name,
                Color = OxyColor.FromRgb(57, 58, 59), //black
                IncreasingFill = OxyColor.FromRgb(17, 178, 64), //green
                DecreasingFill = OxyColor.FromRgb(178, 36, 35), //red
                DataFieldX = "Time",
                DataFieldHigh = "High",
                DataFieldLow = "Low",
                DataFieldOpen = "Open",
                DataFieldClose = "Close",
                TrackerFormatString = "{1:MM/dd/yyyy HH:mm:ss}\nOpen: {4:N4}\nHigh: {2:N4}\nLow: {3:N4}\nClose: {5:N4}",
                ItemsSource = (Ticks ?? new ObservableCollection<TickData>())
            };
            pm.Series.Add(candleStickSeries);
            return pm;
        }
    }
}
