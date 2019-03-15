using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reporter;

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
    }
}
