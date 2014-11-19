using System;
using xAPI.Codes;

namespace xAPI.Responses
{
    using xAPI.Records;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class GetOrderStatusResponse : APIResponse
    {
        /// <summary>
        /// Order status record.
        /// </summary>
        public OrderStatusRecord OrderStatus { get; set; }

        public GetOrderStatusResponse(string body)
            : base(body)
		{
            this.OrderStatus = new OrderStatusRecord((JSONObject)this.ReturnData);
		}
	}
}