using kokos.WPF.Analysis;
using kokos.WPF.ServerConnect;
using kokos.WPF.ViewModel.Base;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using xAPI.Codes;

namespace kokos.WPF.ViewModel
{
    public class SymbolViewModel : AReactiveViewModel
    {
        private static string _lastLoadedDuration;

        public string Duration
        {
            get { return _lastLoadedDuration; }
            set
            {
                SetValue(value);
                _lastLoadedDuration = value;
            }
        }

        public string CategoryName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Description
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

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

        public bool IsLoaded
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public PlotModel Plot
        {
            get { return GetValue<PlotModel>(); }
            set { SetValue(value); }
        }

        public IPlotController PlotController
        {
            get { return GetValue<IPlotController>(); }
            set { SetValue(value); }
        }

        public PlotModel PlotAnalysis
        {
            get { return GetValue<PlotModel>(); }
            set { SetValue(value); }
        }

        public IPlotController PlotAnalysisController
        {
            get { return GetValue<IPlotController>(); }
            set { SetValue(value); }
        }

        public ObservableCollection<TickData> Ticks { get; private set; } 

        public IReactiveCommand LoadTickData { get; private set; }

        public SymbolViewModel()
        {
            if (this.IsInDesignMode())
                IsBusy = true;

            Duration = "3m";
            Ticks = new ObservableCollection<TickData>();
            LoadTickData = ReactiveCommand.CreateAsync(this.WhenAny(x => x.IsBusy, x => !x.Value && !string.IsNullOrEmpty(Name)),
                    ExecuteLoadTickDataAsync, RxApp.MainThreadScheduler);

            PlotController = CreatePlotController();
            PlotAnalysisController = CreatePlotController();

            UpdatePlot("3m");
        }

        private async Task<bool> ExecuteLoadTickDataAsync(object parameter)
        {
            IsBusy = true;

            _lastLoadedDuration = (parameter as string) ?? _lastLoadedDuration;

            PERIOD_CODE periodCode;
            DateTime startDate, endDate;

            GetTickDataInfo(ref _lastLoadedDuration, out startDate, out endDate, out periodCode);
            Duration = _lastLoadedDuration;

            var tickCount = 1000000;

            var ticks = await Task.Run(() => SyncApiWrapper.Instance.LoadData(Name, periodCode, startDate, endDate, tickCount));

            Ticks.Clear();
            foreach (var tick in ticks)
                Ticks.Add(tick);

            UpdatePlot(_lastLoadedDuration);
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
            //else if (duration == "5y")
            //    periodCode = PERIOD_CODE.PERIOD_W1;

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
            var plotModel = CreatePlotModel(Name);

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

            Plot = plotModel;

            var analysisPlotModel = CreatePlotModel("Moving Average");

            var dateValues = Ticks.ToDateValuePoints(x => x.Close).ToList();

            analysisPlotModel.Series.Add(CreateLineSeries("Close Price", OxyColor.FromUInt32(0xCCF0A30A), dateValues));

            analysisPlotModel.Series.Add(CreateLineSeries("MA 10", OxyColor.FromUInt32(0xCCA4C400), BasicAnalysis.CalculateMovingAverage(dateValues, 10)));
            analysisPlotModel.Series.Add(CreateLineSeries("MA 100", OxyColor.FromUInt32(0xCC60A917), BasicAnalysis.CalculateMovingAverage(dateValues, 100)));

            analysisPlotModel.Series.Add(CreateLineSeries("Max 10", OxyColor.FromUInt32(0xCC911A0E), BasicAnalysis.CalculateMax(dateValues, 10)));
            analysisPlotModel.Series.Add(CreateLineSeries("Max 100", OxyColor.FromUInt32(0xCC1569CE), BasicAnalysis.CalculateMax(dateValues, 100)));

            PlotAnalysis = analysisPlotModel;
        }

        private LineSeries CreateLineSeries(string title, OxyColor color, IEnumerable<DateValue> dateValues)
        {
            var lineSeries = new LineSeries { Title = title, Color = color };

            var dataPoints = dateValues.ToDataPoints();

            lineSeries.Points.AddRange(dataPoints);

            return lineSeries;
        }

        private static PlotModel CreatePlotModel(string title)
        {
            var plotModel = new PlotModel { Title = title, LegendSymbolLength = 24 };

            var darkBlue = OxyColors.DarkBlue;

            var timeSpanAxis1 = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "MM/dd/yyyy",
                IsPanEnabled = false,
                IsZoomEnabled = false,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColor.FromAColor(40, darkBlue),
                MinorGridlineColor = OxyColor.FromAColor(20, darkBlue)
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
                MajorGridlineColor = OxyColor.FromAColor(40, darkBlue),
                MinorGridlineColor = OxyColor.FromAColor(20, darkBlue)
            };
            plotModel.Axes.Add(linearAxis1);

            return plotModel;
        }

        private static PlotController CreatePlotController()
        {
            // create a new plot controller with default bindings
            var plotController = new PlotController();

            // add a tracker command to the mouse enter event
            plotController.BindMouseEnter(PlotCommands.HoverPointsOnlyTrack);

            return plotController;
        }
    }
}
