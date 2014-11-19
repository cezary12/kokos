using System.Collections.Generic;
using xAPI.Codes;
using System;

namespace xAPI.Records
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;
    using JSONArray = Newtonsoft.Json.Linq.JArray;

    public class StreamingTradeRecord : APIRecord
    {
        /// <summary>
        /// Closed.
        /// </summary>
        public bool Closed { get; set; }

        /// <summary>
        /// Close price in base currency.
        /// </summary>
        public double ClosePrice { get; set; }

        /// <summary>
        /// Null if order is not closed.
        /// </summary>
        public long? CloseTime { get; set; }

        /// <summary>
        /// Comment.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Commission in account currency.
        /// </summary>
        public double Commision { get; set; }

        /// <summary>
        /// The value the customer may provide in order to retrieve it later.
        /// </summary>
        public string CustomComment { get; set; }

        /// <summary>
        /// Number of decimal places.
        /// </summary>
        public int Digits { get; set; }

        /// <summary>
        /// Null if order is not closed.
        /// </summary>
        public long? Expiration { get; set; }

        /// <summary>
        /// Open price in base currency.
        /// </summary>
        public double OpenPrice { get; set; }

        /// <summary>
        /// Open time.
        /// </summary>
        public long OpenTime { get; set; }

        /// <summary>
        /// Order number for opened transaction.
        /// </summary>
        public long Order { get; set; }

        /// <summary>
        /// Transaction id.
        /// </summary>
        public long Order2 { get; set; }

        /// <summary>
        /// Position number (if type is 0 and 2) or transaction parameter (if type is 1).
        /// </summary>
        public long Position { get; set; }

        /// <summary>
        /// Null unless the trade is closed (type=2) or opened (type=0).
        /// </summary>
        public double? Profit { get; set; }

        /// <summary>
        /// Record type.
        /// </summary>
        public RecordType RecordType { get; set; }

        /// <summary>
        /// Operation code.
        /// </summary>
        public Side Side { get; set; }

        /// <summary>
        /// Zero if stop loss is not set (in base currency).
        /// </summary>
        public double SL { get; set; }

        /// <summary>
        /// Trade state, should be used for detecting pending order's cancellation.
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// Storage.
        /// </summary>
        public double Storage { get; set; }

        /// <summary>
        /// Symbol.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Zero if take profit is not set (in base currency).
        /// </summary>
        public double TP { get; set; }

        /// <summary>
        /// Trade transaction type.
        /// </summary>
        public StreamingTradeType TradeType { get; set; }

        /// <summary>
        /// Volume in lots.
        /// </summary>
        public double Volume { get; set; }

        public StreamingTradeRecord(JSONObject body)
        {
            this.Closed = (bool)body["closed"];
            this.ClosePrice = (double)body["closePrice"];
            this.CloseTime = (long?)body["closeTime"];
            this.Comment = (string)body["comment"];
            this.Commision = (double)body["commision"];
            this.CustomComment = (string)body["customComment"];
            this.Digits = (int)body["digits"];
            this.Expiration = (long?)body["expiration"];
            this.OpenPrice = (double)body["openPrice"];
            this.OpenTime = (long)body["openTime"];
            this.Order = (long)body["order"];
            this.Order2 = (long)body["order2"];
            this.Position = (long)body["position"];
            this.Profit = (double?)body["profit"];
            this.RecordType = RecordType.FromCode((int)body["recordType"]);
            this.Side = Side.FromCode((int)body["side"]);
            this.SL = (double)body["sl"];
            this.State = (int)body["state"];
            this.Storage = (double)body["storage"];
            this.Symbol = (string)body["symbol"];
            this.TP = (double)body["tp"];
            this.TradeType = StreamingTradeType.FromCode((int)body["recordType"]);
            this.Volume = (double)body["volume"];
        }
    }
}
