namespace xAPI.Codes
{
    public class TradeType : APICode
    {
        /// <summary>
        /// Market.
        /// </summary>
        public static readonly TradeType MARKET = new TradeType(0);

        /// <summary>
        /// Limit.
        /// </summary>
        public static readonly TradeType LIMIT = new TradeType(1);

        /// <summary>
        /// Stop.
        /// </summary>
        public static readonly TradeType STOP = new TradeType(2);

        public static TradeType FromCode(int code)
        {
            switch (code)
            {
                case 0: return TradeType.MARKET;
                case 1: return TradeType.LIMIT;
                case 2: return TradeType.STOP;
                default: return new TradeType(code);
            }
        }

        private TradeType(int code) 
            : base(code) 
        { 
        }
    }
}
