using System;
using System.Collections.Generic;
using System.Linq;
using kokos.Abstractions;
using xAPI.Records;

namespace kokos.Communication.Extensions
{
    public static class TickDataExtensions
    {
        public static IEnumerable<TickData> Convert(this IEnumerable<RateInfoRecord> rateInfoRecords, long digits)
        {
            return from rateInfoRecord in rateInfoRecords
                   where rateInfoRecord.IsValid()
                   let mult = Math.Pow(10, System.Convert.ToDouble(digits))
                   let open = rateInfoRecord.Open.Value / mult
                   let high = (rateInfoRecord.Open.Value + rateInfoRecord.High.Value) / mult
                   let low = (rateInfoRecord.Open.Value + rateInfoRecord.Low.Value) / mult
                   let close = (rateInfoRecord.Open.Value + rateInfoRecord.Close.Value) / mult
                   let vol = rateInfoRecord.Vol
                   let ctm = rateInfoRecord.Ctm.Value
                   select new TickData(open, high, low, close, vol, ctm.FromUnixMiliseconds());
        }

        private static bool IsValid(this RateInfoRecord ri)
        {
            return ri.Open.IsValid() && ri.High.IsValid() && ri.Low.IsValid() && ri.Close.IsValid() && ri.Ctm != null;
        }

        private static bool IsValid(this double? val)
        {
            return val != null && !double.IsNaN(val.Value);
        }
    }
}
