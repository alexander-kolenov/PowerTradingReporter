using System;
using System.Collections.Generic;
using TradingPlatform;


namespace Reporter
{
    public class DataAcquisition
    {
        public AggregatedData GetAggregatedTrades(DateTime date)
        {
            var ts = new TradingService();

            var trades = ts.GetTrades(date);
            var ad = AgregateTrades(date, trades);
            return ad;
        }

        public AggregatedData AgregateTrades(DateTime date, IEnumerable<Trade> trades)
        {
            AggregatedData ad = new AggregatedData();
            ad.Date = date;

            foreach (var t in trades)
            {
                foreach (var p in t.Periods)
                {
                    ad.Add(p.Period, p.Volume);
                }
            }
            return ad;
        }

    }
}
