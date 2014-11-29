using kokos.Abstractions;
using System.Collections.Generic;
using System.Linq;
using xAPI.Records;

namespace kokos.Communication.Extensions
{
    public static class TickDataExtensions
    {
        public static IEnumerable<TickData> Convert(this IEnumerable<CandleRecord> rateInfoRecords)
        {
            return rateInfoRecords.Select(Convert);
        }

        private static TickData Convert(CandleRecord candleRecord)
        {
            return new TickData(
                candleRecord.Open,
                candleRecord.High,
                candleRecord.Low,
                candleRecord.Close,
                candleRecord.Timestamp.FromUnixMiliseconds());
        }
    }
}
