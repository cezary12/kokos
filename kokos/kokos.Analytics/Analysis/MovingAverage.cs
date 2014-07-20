using kokos.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace kokos.Analytics.Analysis
{
    public static class MovingAverage
    {
        public static List<DateValue> CalculateEma(IList<TickData> items, int count)
        {
            if (!items.Any())
                return new List<DateValue>();

            var startValue = items.Take(count).Average(x => x.Close);
            var k = 2.0/(count + 1);

            var result = new List<DateValue>();

            for (var i = count; i < items.Count; i++)
            {
                var currentTick = items[i];
                var ema = currentTick.Close * k + startValue * (1 - k);
                startValue = ema;

                result.Add(new DateValue { Date = currentTick.Time, Value = ema });
            }

            return result;
        }

        public static List<DateRangeValue> CalculateEmaChannels(IList<TickData> items, int emaCount, int channelCount, double confidenceInterval = 0.95)
        {
            if (!items.Any())
                return new List<DateRangeValue>();

            var ema = CalculateEma(items, emaCount);

            var result = new List<DateRangeValue>();

            var tickWithEma = (from item in items
                join em in ema on item.Time equals em.Date
                select new
                {
                    em.Date,
                    Ema = em.Value,
                    item.Close,
                    Scale = em.Value > item.Close ? em.Value / item.Close : item.Close / em.Value,
                })
                .ToList();

            var quantile = Convert.ToInt32(confidenceInterval * channelCount);

            for (var i = channelCount; i < tickWithEma.Count; i++)
            {
                var currentEma = tickWithEma[i];
                var currentData = tickWithEma.Skip(i - channelCount).Take(channelCount).Select(x => x.Scale);

                var coeff = currentData.OrderBy(x => x).ElementAt(quantile);

                result.Add(new DateRangeValue
                {
                    Date = currentEma.Date,
                    LowerValue = currentEma.Ema * (2 - coeff),
                    UpperValue = currentEma.Ema * coeff
                });
            }

            return result;
        }

        //private static bool IsOutlier(double ema, double close, double coeff)
        //{
        //    return close < coeff*(1 + ema)
        //           && close > coeff*(1 - ema);
        //}
    }
}
