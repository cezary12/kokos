using kokos.WPF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using xAPI.Records;

namespace kokos.WPF.ServerConnect
{
    public class TickData
    {
        public double Open { get; private set; }
        public double High { get; private set; }
        public double Low { get; private set; }
        public double Close { get; private set; }
        public double? Vol { get; private set; }
        public long Ctm { get; private set; }
        
        public DateTime Time
        {
            get { return Ctm.FromUnixMiliseconds(); }
        }

        public string TimeString
        {
            get { return Time.ToShortDateString() + " " + Time.ToShortTimeString(); }
        }

        public TickData(double open, double high, double low, double close, double? volume, long ctm)
        {
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Vol = volume;
            Ctm = ctm;
        }
    }

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
                   select new TickData(open, high, low, close, vol, ctm);
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
