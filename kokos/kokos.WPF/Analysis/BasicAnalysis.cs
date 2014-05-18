using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace kokos.WPF.Analysis
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

        public static List<DateValue> CalculateMovingAverage(List<DateValue> items, int days)
        {
            var grouped = GroupByDays(items, days);

            return grouped
                .Select(x => new DateValue {Date = x.Key.Date, Value = x.Value.Average(y => y.Value)})
                .ToList();
        }

        public static List<DateValue> CalculateMax(List<DateValue> items, int days)
        {
            var grouped = GroupByDays(items, days);

            return grouped
                .Select(x => new DateValue { Date = x.Key.Date, Value = x.Value.Max(y => y.Value) })
                .ToList();
        }

        private static List<KeyValuePair<DateValue, List<DateValue>>> GroupByDays(List<DateValue> items, int days)
        {
            var temp = new List<DateValue>();
            var result = new List<KeyValuePair<DateValue, List<DateValue>>>();

            int i = items.Count - 1;
            int j = items.Count - 1;

            for (; i >= 0; i--)
            {
                var last = items[i];

                for (; j >= 0; j--)
                {
                    var current = items[j];
                    if ((last.Date - current.Date).TotalDays < days)
                    {
                        temp.Add(current);
                    }
                    else
                    {
                        result.Insert(0, new KeyValuePair<DateValue, List<DateValue>>(last, temp.ToList()));
                        temp.RemoveAt(0);
                        break;
                    }
                }
            }

            return result;
        }
    }
}
