using kokos.Abstractions;
using kokos.Analytics.Analysis;
using kokos.Analytics.Strategies;
using kokos.Communication.ServerConnect;
using kokos.WPF.Extensions;
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

namespace kokos.WPF.ViewModel
{
    public class SymbolViewModel : AReactiveViewModel
    {
        private static string _lastLoadedDuration;

        public string StatusText
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

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

        public ObservableCollection<PlotViewModel> Plots { get; private set; }

        public ObservableCollection<TickData> Ticks { get; private set; }

        public IReactiveCommand LoadTickData { get; private set; }

        public SymbolViewModel()
        {
            StatusText = "Ready";

            if (IsInDesignMode)
                IsBusy = true;

            Duration = "3m";
            Plots = new ObservableCollection<PlotViewModel>();
            Ticks = new ObservableCollection<TickData>();
            LoadTickData = ReactiveCommand.CreateAsyncTask(this.WhenAny(x => x.IsBusy, x => !x.Value && !string.IsNullOrEmpty(Name)),
                    ExecuteLoadTickDataAsync, RxApp.MainThreadScheduler);

            UpdatePlot("3m");
        }

        private async Task<bool> ExecuteLoadTickDataAsync(object parameter)
        {
            IsBusy = true;
            StatusText = "Loading...";

            _lastLoadedDuration = (parameter as string) ?? _lastLoadedDuration;

            DataPeriod periodCode;
            DateTime startDate, endDate;

            GetTickDataInfo(ref _lastLoadedDuration, out startDate, out endDate, out periodCode);
            Duration = _lastLoadedDuration;

            var tickCount = 1000000;

            var ticks = await Task.Run(() => SyncApiWrapper.Instance.LoadData(Name, periodCode, startDate, endDate, tickCount));

            Ticks.Clear();
            foreach (var tick in ticks)
                Ticks.Add(tick);

            UpdatePlot(_lastLoadedDuration);

            StatusText = "Ready";
            IsBusy = false;
            IsLoaded = true;

            return true;
        }

        private static void GetTickDataInfo(ref string duration, out DateTime startDate, out DateTime endDate,
            out DataPeriod periodCode)
        {
            if (string.IsNullOrEmpty(duration))
                duration = "3m";

            periodCode = DataPeriod.D1;

            if (duration == "1d")
                periodCode = DataPeriod.M5;
            else if (duration == "1w")
                periodCode = DataPeriod.H1;
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
            Plots.Clear();

            if (Ticks == null || !Ticks.Any())
                return;

            var candles = CreatePlot(Name, CreateCandleStickSeries(Name + " " + duration, Ticks));

            var dateValues = Ticks.ToDateValuePoints(x => x.Close).ToList();

            var ema = MovingAverage.CalculateEma(Ticks, 22);
            var channels = MovingAverage.CalculateEmaChannels(Ticks, 22, 60);
            var lowerChannel = channels.Select(x => new DateValue {Date = x.Date, Value = x.LowerValue}).ToList();
            var upperChannel = channels.Select(x => new DateValue {Date = x.Date, Value = x.UpperValue}).ToList();

            var plotChannels = CreatePlot("Channels",
                CreateLineSeries("Close Price", OxyColor.FromUInt32(0xCCF0A30A), dateValues),
                CreateLineSeries("EMA 22", OxyColor.FromUInt32(0xCCFA6800), ema),
                CreateLineSeries("Lower Channel", OxyColor.FromUInt32(0xCCA4C400), lowerChannel),
                CreateLineSeries("Upper Channel", OxyColor.FromUInt32(0xCC60A917), upperChannel));

            var plotMa = CreatePlot("Moving Average",
                CreateLineSeries("Close Price", OxyColor.FromUInt32(0xCCF0A30A), dateValues),
                CreateLineSeries("MA 10", OxyColor.FromUInt32(0xCCA4C400), BasicAnalysis.CalculateMovingAverage(Ticks, 10)),
                CreateLineSeries("MA 100", OxyColor.FromUInt32(0xCC60A917), BasicAnalysis.CalculateMovingAverage(Ticks, 100)));

            var plotMinMax = CreatePlot("Min max",
                CreateLineSeries("Close Price", OxyColor.FromUInt32(0xCCF0A30A), dateValues),
                CreateLineSeries("Max 50", OxyColor.FromUInt32(0xCCA4C400), BasicAnalysis.CalculateMax(Ticks, 50)),
                CreateLineSeries("Min 50", OxyColor.FromUInt32(0xCC60A917), BasicAnalysis.CalculateMin(Ticks, 50)));

            var returns = CreatePlot("Returns", "P4",
                CreateTwoColorLineSeries("Annualized Returns", OxyColor.FromUInt32(0xCC60A917), OxyColor.FromUInt32(0xCCFA6800), CalculateReturns(dateValues)));

            Plots.Add(new PlotViewModel(candles));
            Plots.Add(new PlotViewModel(plotChannels));
            Plots.Add(new PlotViewModel(plotMa));
            Plots.Add(new PlotViewModel(plotMinMax));
            Plots.Add(new PlotViewModel(returns));

            //analysisPlotModel.Series.Add(closePrices);

            //analysisPlotModel.Series.Add(CreateLineSeries("MA 10", OxyColor.FromUInt32(0xCCA4C400), BasicAnalysis.CalculateMovingAverage(Ticks, 10)));
            //analysisPlotModel.Series.Add(CreateLineSeries("MA 100", OxyColor.FromUInt32(0xCC60A917), BasicAnalysis.CalculateMovingAverage(Ticks, 100)));

            //analysisPlotModel.Series.Add(CreateLineSeries("Max 10", OxyColor.FromUInt32(0xCC911A0E), BasicAnalysis.CalculateMax(Ticks, 10)));
            //analysisPlotModel.Series.Add(CreateLineSeries("Max 100", OxyColor.FromUInt32(0xCC1569CE), BasicAnalysis.CalculateMax(Ticks, 100)));

            //if (dateValues.Count > 30)
            //{
            //    var anno1 = new TextAnnotation();
            //    anno1.Text = "sdkjfhsdjkfhsd";
            //    anno1.TextPosition = DateTimeAxis.CreateDataPoint(dateValues[10].Date, dateValues[10].Value);
            //    analysisPlotModel.Annotations.Add(anno1);

            //    var anno2 = new ArrowAnnotation();
            //    anno2.Text = "bla blas bla";
            //    anno2.EndPoint = DateTimeAxis.CreateDataPoint(dateValues[30].Date, dateValues[30].Value);
            //    anno2.ArrowDirection = new ScreenVector(50, anno2.EndPoint.Y * 0.3);
            //    analysisPlotModel.Annotations.Add(anno2);
            //}

            //Plots.Add(new PlotViewModel(analysisPlotModel));

            var clenow = new SimpleClenow(Ticks);
        }

        private static List<DateValue> CalculateReturns(IList<DateValue> dateValues)
        {
            var returns = new List<DateValue>();

            for (int i = 1; i < dateValues.Count; i++)
            {
                var days = (dateValues[i].Date - dateValues[i - 1].Date).TotalDays;
                var yf = days/252;
                returns.Add(new DateValue
                {
                    Date = dateValues[i].Date,
                    Value = Math.Log(dateValues[i].Value / dateValues[i - 1].Value) / yf
                });
            }

            return returns;
        }

        private static PlotModel CreatePlot(string title, string xaxisStringFormat, params Series[] lineSeries)
        {
            var plotModel = CreatePlotModel(title, xaxisStringFormat);

            foreach (var series in lineSeries)
                plotModel.Series.Add(series);

            return plotModel;
        }

        private static PlotModel CreatePlot(string title, params Series[] lineSeries)
        {
            return CreatePlot(title, "N4", lineSeries);
        }

        private static CandleStickSeries CreateCandleStickSeries(string title, IEnumerable<TickData> ticks)
        {
            return new CandleStickSeries
            {
                //CandleWidth = 6,
                Title = title,
                Color = OxyColor.FromRgb(57, 58, 59), //black
                IncreasingFill = OxyColor.FromUInt32(0xCC60A917), //green
                DecreasingFill = OxyColor.FromUInt32(0xCCFA6800), //orange
                DataFieldX = "Time",
                DataFieldHigh = "High",
                DataFieldLow = "Low",
                DataFieldOpen = "Open",
                DataFieldClose = "Close",
                TrackerFormatString = "{1:MM/dd/yyyy HH:mm:ss}\nOpen: {4:N4}\nHigh: {2:N4}\nLow: {3:N4}\nClose: {5:N4}",
                ItemsSource = ticks
            };
        }

        private static LineSeries CreateLineSeries(string title, OxyColor color, IEnumerable<DateValue> dateValues)
        {
            var lineSeries = new LineSeries { Title = title, Color = color };

            var dataPoints = dateValues.ToDataPoints();

            lineSeries.Points.AddRange(dataPoints);

            return lineSeries;
        }

        private static TwoColorLineSeries CreateTwoColorLineSeries(string title, OxyColor color, OxyColor color2, IEnumerable<DateValue> dateValues)
        {
            var lineSeries = new TwoColorLineSeries { Title = title, Color = color, Color2 = color2 };

            var dataPoints = dateValues.ToDataPoints();

            lineSeries.Points.AddRange(dataPoints);

            return lineSeries;
        }

        private static PlotModel CreatePlotModel(string title, string xaxisStringFormat)
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
                StringFormat = xaxisStringFormat,
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
    }
}
