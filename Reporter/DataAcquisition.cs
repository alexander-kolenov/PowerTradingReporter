using System;
using System.Collections.Generic;
using TradingPlatform;


namespace Reporter
{
    public class DataAcquisition
    {
        public DateTime GetTradingDay(DateTime utcTime, SessionInfo sessionInfo) // TODO: fix me
        {
            return (utcTime.TimeOfDay >= sessionInfo.SessionStart)
                ? utcTime.Date.AddDays(1)
                : utcTime.Date;
        }

        public AggregatedData GetAggregatedTrades(DateTime utcTime, SessionInfo sessionInfo)
        {
            var ts = new TradingService();

            DateTime tradingDate = GetTradingDay(utcTime, sessionInfo);
            var trades = ts.GetTrades(tradingDate);
            var ad = AgregateTrades(tradingDate, sessionInfo, trades);
            return ad;
        }

        public AggregatedData AgregateTrades(DateTime date, SessionInfo sessionInfo, IEnumerable<Trade> trades)
        {
            AggregatedData ad = new AggregatedData();
            ad.TradingDate = date;
            ad.SessionStart = sessionInfo.SessionStart;

            foreach (var t in trades)
            {
                foreach (var p in t.Periods)
                {
                    ad.AddVolumeToPaticularPeriod(p.Period, p.Volume);
                }
            }
            return ad;
        }
    }
}
