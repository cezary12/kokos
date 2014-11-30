using kokos.Abstractions;
using System.Collections.Generic;

namespace kokos.Communication.Store
{
    public class SymbolStore
    {
        public Symbol Symbol { get; set; }
        public List<TickData> DailyTickData { get; set; }
        public List<TickData> IntradayTickData { get; set; }
 
    }
}
