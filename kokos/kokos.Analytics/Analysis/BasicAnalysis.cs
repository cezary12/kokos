using System;
using System.Collections.Generic;
using System.Linq;
using kokos.Abstractions;

namespace kokos.Analytics.Analysis
{
    public class BasicAnalysis
    {
        //public static List<DateValue> CalculateMovingAverage(List<DateValue> items, int days)
        //{
        //    var temp = new List<DateValue>(items.Take(days));
        //    if (temp.Count < days)
        //        return new List<DateValue>();

        //    var result = new List<DateValue>();

        //    var last = temp.Last();
        //    for (int i = days; i < items.Count; i++)
        //    {
        //        result.Add(new DateValue {Date = last.Date, Value = temp.Average(x => x.Value)});
        //        temp.RemoveAt(0);
                
        //        last = items[i];
        //        temp.Add(last);
        //    }

        //    return result;
        //}

        public static List<DateValue> CalculateMovingAverage(IList<TickData> items, int days)
        {
            var grouped = GroupByDays(items, days);

            return grouped
                .Select(x => new DateValue {Date = x.Key.Time, Value = x.Value.Average(y => y.Close)})
                .ToList();
        }

        public static List<DateValue> CalculateMax(IList<TickData> items, int days)
        {
            var grouped = GroupByDays(items, days);

            return grouped
                .Select(x => new DateValue { Date = x.Key.Time, Value = x.Value.Max(y => y.Close) })
                .ToList();
        }

        public static List<DateValue> CalculateMin(IList<TickData> items, int days)
        {
            var grouped = GroupByDays(items, days);

            return grouped
                .Select(x => new DateValue { Date = x.Key.Time, Value = x.Value.Min(y => y.Close) })
                .ToList();
        }

        public static List<DateValue> CalculateAtr(IList<TickData> items, int days)
        {
            var grouped = GroupByDays(items, days + 1);

            return grouped
                .Select(x =>
                {
                    var trs = new List<double>();

                    for (int i = 1; i < x.Value.Count; i++)
                    {
                        trs.Add(CalculateTr(x.Value[i], x.Value[i-1]));
                    }

                    return new DateValue { Date = x.Key.Time, Value = trs.Average()};
                })
                .ToList();
        }

        private static double CalculateTr(TickData tick, TickData previousTick)
        {
            return Math.Max(previousTick.Close, tick.High) - Math.Min(previousTick.Close, tick.Low);
        }

        private static List<KeyValuePair<TickData, List<TickData>>> GroupByDays(IList<TickData> items, int days)
        {
            var temp = new List<TickData>();
            var result = new List<KeyValuePair<TickData, List<TickData>>>();

            int i = items.Count - 1;
            int j = items.Count - 1;

            for (; i >= 0; i--)
            {
                var last = items[i];

                for (; j >= 0; j--)
                {
                    var current = items[j];
                    if ((last.Time - current.Time).TotalDays < days)
                    {
                        temp.Add(current);
                    }
                    else
                    {
                        result.Insert(0, new KeyValuePair<TickData, List<TickData>>(last, temp.ToList()));
                        temp.RemoveAt(0);
                        break;
                    }
                }
            }

            return result;
        }
    }
}
