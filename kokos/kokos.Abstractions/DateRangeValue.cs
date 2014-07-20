using System;
using System.Diagnostics;

namespace kokos.Abstractions
{
    [DebuggerDisplay("{GetDebuggerDisplay()}")]
    public class DateRangeValue
    {
        public DateTime Date { get; set; }
        public double LowerValue { get; set; }
        public double UpperValue { get; set; }

        private string GetDebuggerDisplay()
        {
            return Date.ToShortDateString() + " " + LowerValue.ToString("N4") + " " + UpperValue.ToString("N4");
        }
    }
}
