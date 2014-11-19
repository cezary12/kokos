namespace xAPI.Codes
{
    public class TradeState : APICode
    {
        /// <summary>
        /// Trade modified.
        /// </summary>
        public static readonly TradeState MODIFIED = new TradeState(0L);

        /// <summary>
        /// Trade deleted.
        /// </summary>
        public static readonly TradeState DELETED = new TradeState(1L);

        public static TradeState FromCode(int code)
        {
            switch (code)
            {
                case 0: return TradeState.MODIFIED;
                case 1: return TradeState.DELETED;
                default: return new TradeState(code);
            }
        }

        private TradeState(long code) 
            : base(code) 
        { 
        }
    }
}