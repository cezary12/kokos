using System;

namespace kokos.Abstractions
{
    public class TickData
    {
        public double Open { get; private set; }
        public double High { get; private set; }
        public double Low { get; private set; }
        public double Close { get; private set; }

        public DateTime Time { get; private set; }

        public string TimeString
        {
            get { return Time.ToShortDateString() + " " + Time.ToShortTimeString(); }
        }

        public TickData(double open, double high, double low, double close, DateTime time)
        {
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Time = time;
        }
    }
}
