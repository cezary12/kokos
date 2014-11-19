namespace xAPI.Codes
{
    public class MarginMode : APICode
    {
        /// <summary>
        /// Forex.
        /// </summary>
        public static readonly MarginMode FOREX = new MarginMode(101L);

        /// <summary>
        /// CFD leveraged.
        /// </summary>
        public static readonly MarginMode CFD_LEVERAGED = new MarginMode(102L);

        /// <summary>
        /// CFD.
        /// </summary>
        public static readonly MarginMode CFD = new MarginMode(103L);

        public static MarginMode FromCode(int code)
        {
            switch (code)
            {
                case 101: return MarginMode.FOREX;
                case 102: return MarginMode.CFD_LEVERAGED;
                case 103: return MarginMode.CFD;
                default: return new MarginMode(code);
            }
        }

        private MarginMode(long code) 
            : base(code) 
        { 
        }
    }
}
