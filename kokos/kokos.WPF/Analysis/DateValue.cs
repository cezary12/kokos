using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using kokos.WPF.ServerConnect;
using OxyPlot;
using OxyPlot.Axes;

namespace kokos.WPF.Analysis
{
    [DebuggerDisplay("{GetDebuggerDisplay()}")]
    public class DateValue
    {
        public DateTime Date { get; set; }
        public double Value { get; set; }

        private string GetDebuggerDisplay()
        {
            return Date.ToShortDateString() + " " + Value.ToString("N4");
        }
    }

    public static class DateValueExtensions
    {
        public static IEnumerable<DateValue> ToDateValuePoints(this IEnumerable<TickData> ticks, Func<TickData, double> valueSelector)
        {
            return (ticks ?? new List<TickData>())
                .Select(x => new DateValue { Date = x.Time, Value = valueSelector(x) });
        }

        public static IEnumerable<DataPoint> ToDataPoints(this IEnumerable<DateValue> dateValues)
        {
            return (dateValues ?? new List<DateValue>())
                .Select(x => DateTimeAxis.CreateDataPoint(x.Date, x.Value));
        }
    }
}
