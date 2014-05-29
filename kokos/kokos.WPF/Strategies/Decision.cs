using System;
using System.Diagnostics;

namespace kokos.WPF.Strategies
{
    [DebuggerDisplay("{GetDebuggerDisplay()}")]
    public class Decision
    {
        public DateTime Date { get; set; }
        public DecisionType DecisionType { get; set; }
        public double Price { get; set; }

        public Decision(DateTime date, DecisionType decisionType, double price)
        {
            Date = date;
            DecisionType = decisionType;
            Price = price;
        }

        private string GetDebuggerDisplay()
        {
            return Date.ToShortDateString() + " " + DecisionType + " " + Price.ToString("N4");
        }
    }
}
