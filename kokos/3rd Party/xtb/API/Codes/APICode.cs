using System;

namespace xAPI.Codes
{
    /// <summary>
    /// Base class for all xAPI codes.
    /// </summary>
    public class APICode
    {
        /// <summary>
        /// Creates new base code object.
        /// </summary>
        /// <param name="code">Code represented as long value.</param>
        public APICode(long code)
        {
            this.Code = code;
        }

        /// <summary>
        /// Raw code received from the API.
        /// </summary>
        public long Code { get; set; }

        public static bool operator ==(APICode baseCode1, APICode baseCode2)
        {
            if(ReferenceEquals(baseCode1, baseCode2))
                return true;

            if((object)baseCode1 == null || (object)baseCode2 == null)
                return false;

            return (baseCode1.Code == baseCode2.Code);
        }

        public static bool operator !=(APICode baseCode1, APICode baseCode2)
        {
            return !(baseCode1 == baseCode2);
        }

        public override bool Equals(object target)
        {
            if (target == null)
                return false;

            APICode baseCode = target as APICode;
            if ((object)baseCode == null)
                return false;

            return (this.Code == baseCode.Code);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
