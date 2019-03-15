using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reporter;
using TradingPlatform;

namespace UnitTestsForReporter
{
    [TestClass]
    public class DataAcquisitionTests
    {

        [TestMethod]
        public void TestMethod1()
        {
            var da = new DataAcquisition();
            SessionInfo si = new SessionInfo();
            si.SessionStart = TimeSpan.FromHours(15);

            DateTime now = new DateTime(2000, 5, 6, 14, 0, 0);
            DateTime tradingDay = da.GetTradingDay(now, si);
            Assert.AreEqual(tradingDay, new DateTime(2000, 5, 6));
        }

        [TestMethod]
        public void TestMethod2()
        {
            var da = new DataAcquisition();
            SessionInfo si = new SessionInfo();
            si.SessionStart = TimeSpan.FromHours(15);

            DateTime now = new DateTime(2000, 5, 6, 18, 0, 0);
            DateTime tradingDay = da.GetTradingDay(now, si);
            Assert.AreEqual(tradingDay, new DateTime(2000, 5, 7));
        }

        [TestMethod]
        public void TestMethod3()
        {
            var da = new DataAcquisition();
            SessionInfo si = new SessionInfo();
            si.SessionStart = TimeSpan.FromHours(15);

            DateTime now = new DateTime(2000, 5, 6, 15, 0, 0);
            DateTime tradingDay = da.GetTradingDay(now, si);
            Assert.AreEqual(tradingDay, new DateTime(2000, 5, 7));
        }

        [TestMethod]
        public void TestMethod4()
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
