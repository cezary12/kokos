using System;

namespace xAPI.Errors
{
	public class APIReplyParseException : APIException
	{
		public APIReplyParseException()
		{
		}

		public APIReplyParseException(string msg) : base(msg)
		{
		}
	}
}