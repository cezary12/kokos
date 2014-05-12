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

        private IPlotController _plotController;
        public IPlotController PlotController
        {
            get { return _plotController; }
            set { this.RaiseAndSetIfChanged(ref _plotController, value); }
        }

        public ObservableCollection<TickData> Ticks { get; private set; } 

        public IReactiveCommand LoadTickData { get; private set; }

        public SymbolViewModel()
        {
            if (this.IsInDesignMode())
                IsBusy = true;

            Ticks = new ObservableCollection<TickData>();
            LoadTickData = ReactiveCommand.CreateAsync(this.WhenAny(x => x.IsBusy, x => !x.Value && !string.IsNullOrEmpty(Name)),
                    ExecuteLoadTickDataAsync, RxApp.MainThreadScheduler);

            UpdatePlot("3m");
        }

        private async Task<bool> ExecuteLoadTickDataAsync(object parameter)
        {
            IsBusy = true;

            var duration = parameter as string;

            PERIOD_CODE periodCode;
            DateTime startDate, endDate;

            GetTickDataInfo(ref duration, out startDate, out endDate, out periodCode);

            var tickCount = 1000000;

            var ticks = await Task.Run(() => SyncApiWrapper.Instance.LoadData(Name, periodCode, startDate, endDate, tickCount));

            Ticks.Clear();
            foreach (var tick in ticks)
                Ticks.Add(tick);

            UpdatePlot(duration);
            IsBusy = false;
            IsLoaded = true;

            return true;
        }

        private static void GetTickDataInfo(ref string duration, out DateTime startDate, out DateTime endDate,
            out PERIOD_CODE periodCode)
        {
            if (string.IsNullOrEmpty(duration))
                duration = "3m";

            periodCode = PERIOD_CODE.PERIOD_D1;

            if (duration == "1d")
                periodCode = PERIOD_CODE.PERIOD_M5;
            else if (duration == "1w")
                periodCode = PERIOD_CODE.PERIOD_H1;
            else if (duration == "5y")
                periodCode = PERIOD_CODE.PERIOD_W1;

            endDate = DateTime.Now;

            if (duration == "5y")
                startDate = endDate.AddYears(-5);
            else if (duration == "12m")
                startDate = endDate.AddMonths(-12);
            else if (duration == "6m")
                startDate = endDate.AddMonths(-6);
            else if (duration == "3m")
                startDate = endDate.AddMonths(-3);
            else if (duration == "1m")
                startDate = endDate.AddMonths(-1);
            else if (duration == "1w")
                startDate = endDate.AddDays(-7);
            else
                startDate = endDate.AddDays(-1);

            startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);
        }

        private void UpdatePlot(string duration)
        {
            var plotModel = new PlotModel { Title = Name, LegendSymbolLength = 24 };

            var c = OxyColors.DarkBlue;
            var timeSpanAxis1 = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "MM/dd/yyyy",
                IsPanEnabled = false,
                IsZoomEnabled = false,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColor.FromAColor(40, c),
                MinorGridlineColor = OxyColor.FromAColor(20, c)
            };
            plotModel.Axes.Add(timeSpanAxis1);

            var linearAxis1 = new LinearAxis
            {
                Position = AxisPosition.Left,
                StringFormat = "N4",
                IsPanEnabled = false,
                IsZoomEnabled = false,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColor.FromAColor(40, c),
                MinorGridlineColor = OxyColor.FromAColor(20, c)
            };
            plotModel.Axes.Add(linearAxis1);

            var lime = OxyColor.FromUInt32(0xCCA4C400);
            var amber = OxyColor.FromUInt32(0xCCF0A30A);

            var green = OxyColor.FromUInt32(0xCC60A917);
            var orange = OxyColor.FromUInt32(0xCCFA6800);

            var increasingFill = green;
            var decreasingFill = orange;

            var candleStickSeries = new CandleStickSeries
            {
                //CandleWidth = 6,
                Title = Name + " " + duration,
                Color = OxyColor.FromRgb(57, 58, 59), //black
                IncreasingFill = increasingFill,
                DecreasingFill = decreasingFill,
                DataFieldX = "Time",
                DataFieldHigh = "High",
                DataFieldLow = "Low",
                DataFieldOpen = "Open",
                DataFieldClose = "Close",
                TrackerFormatString = "{1:MM/dd/yyyy HH:mm:ss}\nOpen: {4:N4}\nHigh: {2:N4}\nLow: {3:N4}\nClose: {5:N4}",
                ItemsSource = (Ticks ?? new ObservableCollection<TickData>())
            };
            plotModel.Series.Add(candleStickSeries);

            // create a new plot controller with default bindings
            var plotController = new PlotController();

            // add a tracker command to the mouse enter event
            plotController.BindMouseEnter(PlotCommands.HoverPointsOnlyTrack);

            Plot = plotModel;
            PlotController = plotController;
        }
    }
}
