namespace xAPI.Codes
{
    public class PeriodCode : APICode
    {
        public static PeriodCode Minutes(int minutes)
        {
            return new PeriodCode(minutes);
        }

        public static PeriodCode Hours(int hours)
        {
            return new PeriodCode(hours * 60);
        }

        public static PeriodCode Days(int days)
        {
            return new PeriodCode(days * 24 * 60);
        }

        public static PeriodCode Weeks(int weeks)
        {
            return new PeriodCode(weeks * 7 * 24 * 60);
        }

        public PeriodCode(long code) 
            : base(code) 
        { 
        }
    }
}