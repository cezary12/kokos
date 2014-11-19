namespace xAPI.Codes
{
    public class StreamingTradeType : APICode
    {
        /// <summary>
        /// Order open, used for opening orders.
        /// </summary>
        public static readonly StreamingTradeType OPEN = new StreamingTradeType(0L);

        /// <summary>
        /// Order pending, only used in the streaming getTrades command.
        /// </summary>
        public static readonly StreamingTradeType PENDING = new StreamingTradeType(1L);

        /// <summary>
        /// Order close.
        /// </summary>
        public static readonly StreamingTradeType CLOSE = new StreamingTradeType(2L);

        /// <summary>
        /// Order modify, only used in the tradeTransaction command.
        /// </summary>
        public static readonly StreamingTradeType MODIFY = new StreamingTradeType(3L);

        /// <summary>
        /// Order delete, only used in the tradeTransaction command.
        /// </summary>
        public static readonly StreamingTradeType DELETE = new StreamingTradeType(4L);

        public static StreamingTradeType FromCode(int code)
        {
            switch (code)
            {
                case 0: return StreamingTradeType.OPEN;
                case 1: return StreamingTradeType.PENDING;
                case 2: return StreamingTradeType.CLOSE;
                case 3: return StreamingTradeType.MODIFY;
                case 4: return StreamingTradeType.DELETE;
                default: return new StreamingTradeType(code);
            }
        }

        private StreamingTradeType(long code) 
            : base(code) 
        { 
        }
    }
}