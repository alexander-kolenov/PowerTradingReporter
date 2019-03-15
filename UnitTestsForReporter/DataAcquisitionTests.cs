using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reporter;
using TradingPlatform;

namespace UnitTestsForReporter
{
    [TestClass]
    public class DataAcquisitionTests
    {

        [TestMethod]
        [Description("Test GetTradingDay")]
        [DataRow("15:00", "2000-5-6 14:00", "2000-5-6 00:00")]
        [DataRow("15:00", "2000-5-6 15:00", "2000-5-7 00:00")]
        [DataRow("15:00", "2000-5-6 16:00", "2000-5-7 00:00")]
        public void TestMethod1(string sessionStart, string now, string result)
        {

            TimeSpan x_sessionStart = TimeSpan.Parse(sessionStart, CultureInfo.InvariantCulture);
            DateTime x_now = DateTime.Parse(now, CultureInfo.InvariantCulture);
            DateTime x_result = DateTime.Parse(result, CultureInfo.InvariantCulture);

            var da = new DataAcquisition();
            SessionInfo si = new SessionInfo();
            si.SessionStart = x_sessionStart;
            
            DateTime tradingDay = da.GetTradingDay(x_now, si);
            Assert.AreEqual(tradingDay, x_result);
        }


        [TestMethod]
        [Description("Test AgregateTrades")]
        public void TestMethod2()
        {
            var da = new DataAcquisition();
            SessionInfo si = new SessionInfo();
            si.SessionStart = TimeSpan.FromHours(15);
            DateTime now = new DateTime(2000, 5, 6, 15, 0, 0);
            List<Trade> trades = new List<Trade>();
            trades.Add(Trade.Create(now, 1));
            trades.Add(Trade.Create(now, 2));
            trades.Add(Trade.Create(now, 3));

            AggregatedData at = da.AgregateTrades(now, si, trades);

            Assert.AreEqual(at.TradingDate, now);
            Assert.AreEqual(at.SessionStart, si.SessionStart);
            Assert.AreEqual(at.Volumes.Length, 24);
        }

    }
}
