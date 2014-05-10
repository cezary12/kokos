using System;
using kokos.WPF.Utils;
using xAPI.Records;

namespace kokos.WPF.ServerConnect
{
    public class TickData
    {
        private long _digits;

        public double? Open { get; private set; }
        public double? High { get; private set; }
        public double? Low { get; private set; }
        public double? Close { get; private set; }
        public double? Vol { get; private set; }
        public long? Ctm { get; private set; }
        
        public DateTime? Time
        {
            get { return Ctm == null ? (DateTime?) null : Ctm.Value.FromUnixMiliseconds(); }
        }

        public string TimeString
        {
            get { return Time == null ? "null" : (Time.Value.ToShortDateString() + " " + Time.Value.ToShortTimeString()); }
        }

        public TickData(RateInfoRecord rateInfoRecord, long digits)
        {
            var mult = Math.Pow(10, Convert.ToDouble(digits));

            Open = rateInfoRecord.Open/mult;
            High = (rateInfoRecord.Open + rateInfoRecord.High)/mult;
            Low = (rateInfoRecord.Open + rateInfoRecord.Low)/mult;
            Close = (rateInfoRecord.Open + rateInfoRecord.Close)/mult;
            Vol = rateInfoRecord.Vol;
            Ctm = rateInfoRecord.Ctm;

            _digits = digits;
        }
    }
}
