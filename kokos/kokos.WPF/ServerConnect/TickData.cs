using System;
using kokos.WPF.Utils;
using xAPI.Records;

namespace kokos.WPF.ServerConnect
{
    public class TickData
    {
        public double? Open { get; set; }

        public double? High { get; set; }
        public double? Low { get; set; }

        public double? Close { get; set; }

        public double? Vol { get; set; }
        
        public long? Ctm { get; set; }
        
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
        }
    }
}
