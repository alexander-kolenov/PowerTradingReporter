using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPlatform;


namespace Reporter
{
    public class DataAcquisition
    {
        public AggregatedData GetAggregatedTrades(DateTime date)
        {
            var ts = new TradingService();

            DateTime dt = DateTime.UtcNow;
            var trades = ts.GetTrades(dt);
            var ad = AgregateTrades(dt, trades);
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
