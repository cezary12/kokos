using System.Collections.Generic;
using System.Linq;

namespace kokos.WPF.Analysis
{
    public class BasicAnalysis
    {
        public static List<DateValue> CalculateMovingAverage(List<DateValue> items, int length)
        {
            var temp = new List<DateValue>(items.Take(length));
            if (temp.Count < length)
                return new List<DateValue>();

            var result = new List<DateValue>();

            var last = temp.Last();
            for (int i = length; i < items.Count; i++)
            {
                result.Add(new DateValue {Date = last.Date, Value = temp.Average(x => x.Value)});
                temp.RemoveAt(0);
                
                last = items[i];
                temp.Add(last);
            }

            return result;
        }


    }
}
