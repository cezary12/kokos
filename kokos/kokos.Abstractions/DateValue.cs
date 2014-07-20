using System;
using System.Diagnostics;

namespace kokos.Abstractions
{
    [DebuggerDisplay("{GetDebuggerDisplay()}")]
    public class DateValue
    {
        public DateTime Date { get; set; }
        public double Value { get; set; }

        private string GetDebuggerDisplay()
        {
            return Date.ToShortDateString() + " " + Value.ToString("N4");
        }
    }
}
