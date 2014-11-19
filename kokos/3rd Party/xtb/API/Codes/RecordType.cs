namespace xAPI.Codes
{
    public class RecordType : APICode
    {
        /// <summary>
        /// Order open, used for opening orders.
        /// </summary>
        public static readonly RecordType OPEN = new RecordType(0);

        /// <summary>
        /// Order pending, only used in the streaming getTrades command.
        /// </summary>
        public static readonly RecordType PENDING = new RecordType(1);

        /// <summary>
        /// Order close.
        /// </summary>
        public static readonly RecordType CLOSE = new RecordType(2);

        /// <summary>
        /// Order modify, only used in the tradeTransaction command.
        /// </summary>
        public static readonly RecordType MODIFY = new RecordType(3);

        /// <summary>
        /// Order delete, only used in the tradeTransaction command.
        /// </summary>
        public static readonly RecordType DELETE = new RecordType(4);

        public static RecordType FromCode(int code)
        {
            switch (code)
            {
                case 0: return RecordType.OPEN;
                case 1: return RecordType.PENDING;
                case 2: return RecordType.CLOSE;
                case 3: return RecordType.MODIFY;
                case 4: return RecordType.DELETE;
                default: return new RecordType(code);
            }
        }

        private RecordType(int code) 
            : base(code) 
        { 
        }
    }
}
