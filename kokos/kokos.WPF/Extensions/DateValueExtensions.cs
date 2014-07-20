using kokos.Abstractions;
using OxyPlot;
using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace kokos.WPF.Extensions
{
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
