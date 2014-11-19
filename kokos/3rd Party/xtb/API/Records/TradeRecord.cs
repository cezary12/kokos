using System;

namespace xAPI.Records
{
    using xAPI.Codes;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class TradeRecord : APIRecord
    {
        /// <summary>
        /// True if is closed.
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
        /// Null if order is not closed.
        /// </summary>
        public string CloseTimeString { get; set; }

        /// <summary>
        /// Comment.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Commission in account currency.
        /// </summary>
        public double Commission { get; set; }

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
        /// Null if order is not closed.
        /// </summary>
        public string ExpirationString { get; set; }

        /// <summary>
        /// Open price in base currency.
        /// </summary>
        public double OpenPrice { get; set; }

        /// <summary>
        /// Open time.
        /// </summary>
        public long OpenTime { get; set; }

        /// <summary>
        /// Open time as string.
        /// </summary>
        public string OpenTimeString { get; set; }

        /// <summary>
        /// Order number for opened transaction.
        /// </summary>
        public long Order { get; set; }

        /// <summary>
        /// Order number for closed transaction.
        /// </summary>
        public long Order2 { get; set; }

        /// <summary>
        /// Order number common both for opened and closed transaction.
        /// </summary>
        public long Position { get; set; }

        /// <summary>
        /// Profit in account currency.
        /// </summary>
        public double Profit { get; set; }

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
        /// Order swaps in account currency.
        /// </summary>
        public double Storage { get; set; }

        /// <summary>
        /// Symbol name or null for deposit/withdrawal operations.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Timestamp.
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// Zero if take profit is not set (in base currency).
        /// </summary>
        public double TP { get; set; }

        /// <summary>
        /// Trade transaction type.
        /// </summary>
        public int TradeType { get; set; }

        /// <summary>
        /// Volume in lots.
        /// </summary>
        public double Volume { get; set; }

        public TradeRecord(JSONObject body)
        {
            this.Closed = (bool)body["closed"];
            this.ClosePrice = (double)body["closePrice"];
            this.CloseTime = (long?)body["closeTime"];
            this.CloseTimeString = (string)body["closeTimeString"];
            this.Comment = (string)body["comment"];
            this.Commission = (double)body["commission"];
            this.CustomComment = (string)body["customComment"];
            this.Digits = (int)body["digits"];
            this.Expiration = (long?)body["expiration"];
            this.ExpirationString = (string)body["expirationString"];
            this.OpenPrice = (double)body["openPrice"];
            this.OpenTime = (long)body["openTime"];
            this.OpenTimeString = (string)body["openTimeString"];
            this.Order = (long)body["order"];
            this.Order2 = (long)body["order2"];
            this.Position = (long)body["position"];
            this.Profit = (double)body["profit"];
            this.RecordType = RecordType.FromCode((int)body["recordType"]);
            this.Side = Side.FromCode((int)body["side"]);
            this.SL = (double)body["sl"];
            this.Storage = (double)body["storage"];
            this.Symbol = (string)body["symbol"];
            this.Timestamp = (long)body["timestamp"];
            this.TP = (double)body["tp"];
            this.TradeType = (int)body["tradeType"];
            this.Volume = (double)body["volume"];
        }
	}
}