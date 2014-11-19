using System;

namespace xAPI.Errors
{
	public class APICommandConstructionException : APIException
	{
		public APICommandConstructionException()
		{
		}

		public APICommandConstructionException(string msg) : base(msg)
		{
		}
	}
}