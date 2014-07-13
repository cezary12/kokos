using System.Security.Cryptography;
using kokos.WPF.Analysis;
using kokos.WPF.ServerConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kokos.WPF.Strategies
{
    /// <summary>
    ///• Long entries are only allowed if the 50-day moving average is above the 100-day moving average.
    ///• Short entries are only allowed if the 50-day moving average is below the 100-day moving average.
    ///• If today’s closing price is the highest close in the past 50 days, we buy.
    ///• If today’s closing price is the lowest close in the past 50 days, we sell.
    ///• Position sizing is volatility adjusted according to the ATR-based formula previously shown, with a
    ///risk factor of 20 basis points.
    ///• A long position is closed when it has moved three ATR units down from its highest closing price
    ///since the position was opened.
    ///• A short position is closed when it has moved three ATR units up from its lowest closing price since
    ///the position was opened.
    ///• The investment universe consists of five sectors with 10 markets in each.
    /// </summary>
    public class SimpleClenow
    {
        public readonly List<DateValue> IndexLine = new List<DateValue>();

        public SimpleClenow(IList<TickData> ticks)
        {
            var ma50Line = BasicAnalysis.CalculateMovingAverage(ticks, 50);
            var ma100Line = BasicAnalysis.CalculateMovingAverage(ticks, 100);

            var max50Line = BasicAnalysis.CalculateMax(ticks, 50);
            var min50Line = BasicAnalysis.CalculateMin(ticks, 50);

            var atrLine = BasicAnalysis.CalculateAtr(ticks, 100);

            var data = (from ma50 in ma50Line
                join ma100 in ma100Line on ma50.Date equals ma100.Date
                join max50 in max50Line on ma50.Date equals max50.Date
                join min50 in min50Line on ma50.Date equals min50.Date
                join atr in atrLine on ma50.Date equals atr.Date
                join tick in ticks on ma50.Date equals tick.Time
                select new
                {
                    Tick = tick,
                    Ma50 = ma50.Value,
                    Ma100 = ma100.Value,
                    Max50 = max50.Value,
                    Min50 = min50.Value,
                    Atr = atr.Value,

                    LongAllowed = ma50.Value > ma100.Value,
                    ShortAllowed = ma50.Value < ma100.Value,

                    BuySignal = tick.Close >= max50.Value,
                    SellSignal = tick.Close <= min50.Value,
                })
                .ToList();

            var decisions = new List<Tuple<int, Signal>>();

            for (int i = 0; i < data.Count - 1; i++)
            {
                var lastDecision = decisions.LastOrDefault();
                var lastDecisionType = lastDecision != null ? lastDecision.Item2.DecisionType : DecisionType.StayAway;

                var currentPoint = data[i];

                var closePrice = currentPoint.Tick.Close;
                var entryPrice = data[i + 1].Tick.Open;

                if (currentPoint.BuySignal && currentPoint.LongAllowed)
                {
                    if (lastDecisionType == DecisionType.Buy)
                        continue;

                    decisions.Add(new Tuple<int, Signal>(i, new Signal(currentPoint.Tick.Time, DecisionType.Buy, entryPrice)));
                    continue;
                }

                if (currentPoint.SellSignal && currentPoint.ShortAllowed)
                {
                    if (lastDecisionType == DecisionType.Sell)
                        continue;

                    decisions.Add(new Tuple<int, Signal>(i, new Signal(currentPoint.Tick.Time, DecisionType.Sell, entryPrice)));
                    continue;
                }

                if (!currentPoint.LongAllowed && lastDecisionType == DecisionType.Buy)
                {
                    decisions.Add(new Tuple<int, Signal>(i, new Signal(currentPoint.Tick.Time, DecisionType.Close, entryPrice)));
                    continue;
                }

                if (!currentPoint.ShortAllowed && lastDecisionType == DecisionType.Sell)
                {
                    decisions.Add(new Tuple<int, Signal>(i, new Signal(currentPoint.Tick.Time, DecisionType.Close, entryPrice)));
                    continue;
                }

                if (lastDecision != null)
                {
                    //stop loss
                    var ticksRange = data
                        .Skip(lastDecision.Item1)
                        .Take(i - lastDecision.Item1)
                        .ToList();

                    if (lastDecisionType == DecisionType.Buy)
                    {
                        var highestClose = ticksRange.Max(x => x.Tick.Close);
                        highestClose -= 3*currentPoint.Atr;
                        if (closePrice < highestClose)
                        {
                            decisions.Add(new Tuple<int, Signal>(i, new Signal(currentPoint.Tick.Time, DecisionType.Close, entryPrice)));
                            continue;
                        }
                    }

                    if (lastDecisionType == DecisionType.Sell)
                    {
                        var lowestClose = ticksRange.Min(x => x.Tick.Close);
                        lowestClose += 3*currentPoint.Atr;
                        if (closePrice > lowestClose)
                        {
                            decisions.Add(new Tuple<int, Signal>(i, new Signal(currentPoint.Tick.Time, DecisionType.Close, entryPrice)));
                            continue;
                        }
                    }
                }
            }

            var temp = decisions.Select(x => x.Item2).ToList();

            IndexLine.Clear();

            var dataAndDecisions = (from d in data
                join dec in decisions on d.Tick.Time equals dec.Item2.Date into joined
                from jn in joined.DefaultIfEmpty()
                select new
                {
                    Tick = d.Tick,
                    Decision = jn != null ? jn.Item2 : null
                })
                .ToList();

            Signal lastDec = null;

            for (int i = 0; i < dataAndDecisions.Count; i++)
            {
                var currentPoint = dataAndDecisions[i];
                var previousPoint = i == 0 ? currentPoint : dataAndDecisions[i - 1];

                var lastIndexPoint = IndexLine.LastOrDefault();
                var lastIndexValue = lastIndexPoint != null ? lastIndexPoint.Value : 100;

                if (lastDec == null)
                {
                    IndexLine.Add(new DateValue {Date = currentPoint.Tick.Time, Value = lastIndexValue});
                }
                else
                {
                    var perf = currentPoint.Tick.Close / previousPoint.Tick.Close;
                    if (lastDec.DecisionType == DecisionType.Buy)
                    {
                        IndexLine.Add(new DateValue { Date = currentPoint.Tick.Time, Value = lastIndexValue * perf });
                    }

                    if (lastDec.DecisionType == DecisionType.Sell)
                    {
                        IndexLine.Add(new DateValue { Date = currentPoint.Tick.Time, Value = lastIndexValue * (2 - perf) });
                    }
                }

                if (currentPoint.Decision != null)
                {
                    if (currentPoint.Decision.DecisionType == DecisionType.Close)
                    {
                        lastDec = null;
                        continue;
                    }

                    lastDec = currentPoint.Decision;
                }
            }

        }
    }
}
