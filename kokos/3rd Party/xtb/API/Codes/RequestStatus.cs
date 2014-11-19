namespace xAPI.Codes
{
    public class RequestStatus : APICode
    {
        /// <summary>
        /// There was an error in the transaction.
        /// </summary>
        public static readonly RequestStatus ERROR = new RequestStatus(0L);

        /// <summary>
        /// Transaction is still pending.
        /// </summary>
        public static readonly RequestStatus PENDING = new RequestStatus(1L);

        /// <summary>
        /// The transaction has been executed successfully.
        /// </summary>
        public static readonly RequestStatus ACCEPTED = new RequestStatus(3L);

        /// <summary>
        /// The transaction has been rejected.
        /// </summary>
        public static readonly RequestStatus REJECTED = new RequestStatus(4L);

        public static RequestStatus FromCode(int code)
        {
            switch (code)
            {
                case 0: return RequestStatus.ERROR;
                case 1: return RequestStatus.PENDING;
                case 3: return RequestStatus.ACCEPTED;
                case 4: return RequestStatus.REJECTED;
                default: return new RequestStatus(code);
            }
        }

        private RequestStatus(long code) 
            : base(code) 
        { 
        }
    }
}