using Biggy.Core;
using Biggy.Data.Json;

namespace kokos.Communication.Store
{
    public class SymbolDb
    {
        public BiggyList<SymbolStore> Symbols { get; protected set; }

        public SymbolDb()
        {
            Symbols = new BiggyList<SymbolStore>(new JsonStore<SymbolStore>("symboldata"));
        }
    }
}
