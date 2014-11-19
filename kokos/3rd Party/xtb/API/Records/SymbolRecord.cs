using System;
using xAPI.Codes;

namespace xAPI.Records
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class SymbolRecord : APIRecord
	{
        /// <summary>
        /// Ask price in base currency.
        /// </summary>
        public double Ask { get; set; }

        /// <summary>
        /// Bid price in base currency.
        /// </summary>
        public double Bid { get; set; }

        /// <summary>
        /// Category name.
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Size of 1 lot.
        /// </summary>
        public int ContractSize { get; set; }

        /// <summary>
        /// Currency.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Indicates whether the symbol represents a currency pair.
        /// </summary>
        public bool CurrencyPair { get; set; }

        /// <summary>
        /// The currency of calculated profit.
        /// </summary>
        public string CurrencyProfit { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Symbol group name.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// The highest price of the day in base currency.
        /// </summary>
        public double High { get; set; }

        /// <summary>
        /// Symbol leverage.
        /// </summary>
        public double Leverage { get; set; }

        /// <summary>
        /// Maximum size of trade.
        /// </summary>
        public double LotMax { get; set; }

        /// <summary>
        /// Minimum size of trade.
        /// </summary>
        public double LotMin { get; set; }

        /// <summary>
        /// A value of minimum step by which the size of trade can be changed (within lotMin - lotMax range).
        /// </summary>
        public double LotStep { get; set; }

        /// <summary>
        /// The lowest price of the day in base currency.
        /// </summary>
        public double Low { get; set; }

        /// <summary>
        /// For margin calculation.
        /// </summary>
        public MarginMode MarginMode { get; set; }

        /// <summary>
        /// Percentage.
        /// </summary>
        public double Percentage { get; set; }

        /// <summary>
        /// Number of symbol's price decimal places.
        /// </summary>
        public int Precision { get; set; }

        /// <summary>
        /// For profit calculation.
        /// </summary>
        public ProfitMode ProfitMode { get; set; }

        /// <summary>
        /// Source of price.
        /// </summary>
        public int QuoteId { get; set; }

        /// <summary>
        /// The difference between raw ask and bid prices.
        /// </summary>
        public double SpreadRaw { get; set; }

        /// <summary>
        /// Spread representation.
        /// </summary>
        public double SpreadTable { get; set; }

        /// <summary>
        /// Minimal distance (in pips) from the current price where the stopLoss/takeProfit can be set.
        /// </summary>
        public int StopsLevel { get; set; }

        /// <summary>
        /// Indicates whether swap value is added to position on end of day.
        /// </summary>
        public bool SwapEnable { get; set; }

        /// <summary>
        /// Swap value for long positions in pips.
        /// </summary>
        public double SwapLong { get; set; }

        /// <summary>
        /// Swap value for short positions in pips.
        /// </summary>
        public double SwapShort { get; set; }

        /// <summary>
        /// Type of swap calculated.
        /// </summary>
        public int SwapType { get; set; }

        /// <summary>
        /// Symbol name.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Smallest possible price change, used for profit/margin calculation, null if not applicable.
        /// </summary>
        public double? TickSize { get; set; }

        /// <summary>
        /// Value of smallest possible price change (in base currency), used for profit/margin calculation, null if not applicable.
        /// </summary>
        public double? TickValue { get; set; }

        /// <summary>
        /// Ask & bid tick time.
        /// </summary>
        public long Time { get; set; }

        /// <summary>
        /// Time as string value.
        /// </summary>
        public string TimeString { get; set; }

        /// <summary>
        /// Instrument class number.
        /// </summary>
        public int Type { get; set; }

        public SymbolRecord(JSONObject body)
        {
            this.Ask = (double)body["ask"];
            this.Bid = (double)body["bid"];
            this.CategoryName = (string)body["categoryName"];
            this.Currency = (string)body["currency"];
            this.CurrencyPair = (bool)body["currencyPair"];
            this.CurrencyProfit = (string)body["currencyProfit"];
            this.Description = (string)body["description"];
            this.GroupName = (string)body["groupName"];
            this.High = (double)body["high"];
            this.Leverage = (double)body["leverage"];
            this.LotMax = (double)body["lotMax"];
            this.LotMin = (double)body["lotMin"];
            this.LotStep = (double)body["lotStep"];
            this.Low = (double)body["low"];
            this.Precision = (int)body["precision"];
            this.ContractSize = (int)body["contractSize"];
            this.MarginMode = MarginMode.FromCode((int)body["marginMode"]);
            this.Percentage = (double)body["percentage"];
            this.ProfitMode = ProfitMode.FromCode((int)body["profitMode"]);
            this.QuoteId = (int)body["quoteId"];
            this.SpreadRaw = (double)body["spreadRaw"];
            this.SpreadTable = (double)body["spreadTable"];
            this.StopsLevel = (int)body["stopsLevel"];
            this.SwapEnable = (bool)body["swapEnable"];
            this.SwapLong = (double)body["swapLong"];
            this.SwapShort = (double)body["swapShort"];
            this.SwapType = (int)body["swapType"];
            this.Symbol = (string)body["symbol"];
            this.TickSize = (double?)body["tickSize"];
            this.TickValue = (double?)body["tickValue"];
            this.Time = (long)body["time"];
            this.TimeString = (string)body["timeString"];
            this.Type = (int)body["type"];
        }
    }
}