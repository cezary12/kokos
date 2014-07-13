using System;
using System.Diagnostics;

namespace kokos.WPF.Strategies
{
    [DebuggerDisplay("{GetDebuggerDisplay()}")]
    public class Signal
    {
        public DateTime Date { get; set; }
        public DecisionType DecisionType { get; set; }
        public double Price { get; set; }
        public string Comment { get; set; }

        public Signal(DateTime date, DecisionType decisionType, double price, string comment = "")
        {
            Date = date;
            DecisionType = decisionType;
            Price = price;
            Comment = comment;
        }

        private string GetDebuggerDisplay()
        {
            return Date.ToShortDateString() + " " + DecisionType + " " + Price.ToString("N4");
        }
    }
}
