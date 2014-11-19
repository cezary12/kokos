using System;

namespace xAPI.Errors
{
	public class APICommunicationException : APIException
	{
		public APICommunicationException()
		{
		}

		public APICommunicationException(string msg) : base(msg)
		{
		}
	}
}