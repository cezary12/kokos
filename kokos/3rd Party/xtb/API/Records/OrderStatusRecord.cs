using System.Collections.Generic;
using xAPI.Codes;
using System;

namespace xAPI.Records
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;
    using JSONArray = Newtonsoft.Json.Linq.JArray;    

    public class OrderStatusRecord : APIRecord
    {
        /// <summary>
        /// The value the customer may provide in order to retrieve it later.
        /// </summary>
        public string CustomComment { get; private set; }

        /// <summary>
        /// Can be null.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Unique order number.
        /// </summary>
        public long Order { get; private set; }

        /// <summary>
        /// Price in base currency.
        /// </summary>
        public double Price { get; private set; }

        /// <summary>
        /// Request status code, described below.
        /// </summary>
        public RequestStatus RequestStatus { get; private set; }

        public OrderStatusRecord(JSONObject value)
        {
            this.CustomComment = (string)value["customComment"];
            this.Message = (string)value["message"];
            this.Order = (long)value["order"];
            this.Price = (double)value["price"];
            this.RequestStatus = RequestStatus.FromCode((int)value["requestStatus"]);
        }
    }
}
