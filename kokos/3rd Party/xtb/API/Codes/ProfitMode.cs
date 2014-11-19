namespace xAPI.Codes
{
    public class ProfitMode : APICode
    {
        /// <summary>
        /// FOREX.
        /// </summary>
        public static readonly ProfitMode FOREX = new ProfitMode(5L);

        /// <summary>
        /// CFD.
        /// </summary>
	    public static readonly ProfitMode CFD = new ProfitMode(6L);

        public static ProfitMode FromCode(int code)
        {
            switch (code)
            {
                case 5: return ProfitMode.FOREX;
                case 6: return ProfitMode.CFD;
                default: return new ProfitMode(code);
            }
        }

        private ProfitMode(long code) 
            : base(code) 
        { 
        }
    }
}
