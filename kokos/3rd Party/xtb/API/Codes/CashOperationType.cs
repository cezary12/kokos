namespace xAPI.Codes
{
    public class CashOperationType : APICode
    {
        /// <summary>
        /// Deposit.
        /// </summary>
        public static readonly CashOperationType DEPOSIT = new CashOperationType(1);

        /// <summary>
        /// Withdrawal.
        /// </summary>
        public static readonly CashOperationType WITHDRAWAL = new CashOperationType(2);

        /// <summary>
        /// Close trade.
        /// </summary>
        public static readonly CashOperationType CLOSE_TRADE = new CashOperationType(3);

        /// <summary>
        /// Correction.
        /// </summary>
        public static readonly CashOperationType CORRECTION = new CashOperationType(4);

        /// <summary>
        /// Interest.
        /// </summary>
        public static readonly CashOperationType INTEREST = new CashOperationType(5);

        /// <summary>
        /// Tax.
        /// </summary>
        public static readonly CashOperationType TAX = new CashOperationType(6);

        /// <summary>
        /// Swap.
        /// </summary>
        public static readonly CashOperationType SWAP = new CashOperationType(7);

        /// <summary>
        /// Rolling.
        /// </summary>
        public static readonly CashOperationType ROLLING = new CashOperationType(8);

        /// <summary>
        /// Dividend.
        /// </summary>
        public static readonly CashOperationType DIVIDEND = new CashOperationType(9);

        /// <summary>
        /// Rights issue.
        /// </summary>
        public static readonly CashOperationType RIGHTS_ISSUE = new CashOperationType(10);

        /// <summary>
        /// Spinoff.
        /// </summary>
        public static readonly CashOperationType SPINOFF = new CashOperationType(11);

        /// <summary>
        /// Commission.
        /// </summary>
        public static readonly CashOperationType COMMISSION = new CashOperationType(12);

        /// <summary>
        /// Credit in.
        /// </summary>
        public static readonly CashOperationType CREDIT_IN = new CashOperationType(13);

        /// <summary>
        /// Credit out.
        /// </summary>
        public static readonly CashOperationType CREDIT_OUT = new CashOperationType(14);

        /// <summary>
        /// Up-down in.
        /// </summary>
        public static readonly CashOperationType UP_DOWN_IN = new CashOperationType(15);

        /// <summary>
        /// Up-down out.
        /// </summary>
        public static readonly CashOperationType UP_DOWN_OUT = new CashOperationType(16);

        /// <summary>
        /// IB commission.
        /// </summary>
        public static readonly CashOperationType IB_COMMISSION = new CashOperationType(17);

        /// <summary>
        /// Management fee.
        /// </summary>
        public static readonly CashOperationType MANAGEMENT_FEE = new CashOperationType(18);

        /// <summary>
        /// Success fee.
        /// </summary>
        public static readonly CashOperationType SUCCESS_FEE = new CashOperationType(19);

        public static CashOperationType FromCode(int code)
        {
            switch (code)
            {
                case 1: return CashOperationType.DEPOSIT;
                case 2: return CashOperationType.WITHDRAWAL;
                case 3: return CashOperationType.CLOSE_TRADE;
                case 4: return CashOperationType.CORRECTION;
                case 5: return CashOperationType.INTEREST;
                case 6: return CashOperationType.TAX;
                case 7: return CashOperationType.SWAP;
                case 8: return CashOperationType.ROLLING;
                case 9: return CashOperationType.DIVIDEND;
                case 10: return CashOperationType.RIGHTS_ISSUE;
                case 11: return CashOperationType.SPINOFF;
                case 12: return CashOperationType.COMMISSION;
                case 13: return CashOperationType.CREDIT_IN;
                case 14: return CashOperationType.CREDIT_OUT;
                case 15: return CashOperationType.UP_DOWN_IN;
                case 16: return CashOperationType.UP_DOWN_OUT;
                case 17: return CashOperationType.IB_COMMISSION;
                case 18: return CashOperationType.MANAGEMENT_FEE;
                case 19: return CashOperationType.SUCCESS_FEE;
                default: return new CashOperationType(code);
            }
        }

        private CashOperationType(int code) 
            : base(code) 
        { 
        }
    }
}
