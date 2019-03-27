using Csv;
using NLog;
using Reporter;
using System;
using System.IO;
using Unity;

namespace DebugMe
{
    public class Test1 : ITest
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        [Dependency]
        public TradingReporterConfiguration Config { get; set; }

        [Dependency]
        public TradingReporter Reporter { get; set; }

        public void Run()
        {
            DateTime dt = DateTime.UtcNow;

            DataAcquisition da = new DataAcquisition();

            for (int i = 0; i < 10; i++)
            {
                dt = dt.AddHours(1);
                try
                {

                    DateTime tradingDate = da.GetTradingDay(dt, Config.SessionInfo);
                    AggregatedData ad = da.GetAggregatedTrades(tradingDate, Config.SessionInfo);
                    ReportBuilder rb = new ReportBuilder();
                    Directory.CreateDirectory(Config.ReportingDirrectory);
                    string reportFileName = Path.Combine(Config.ReportingDirrectory, rb.GetCsvReportFileName(dt));
                    CsvData csvData = rb.CreateCsvData(ad);

                    using (TextWriter tw = new StreamWriter(reportFileName))
                    {
                        CsvWriter.Write(tw, csvData.Headers, csvData.Rows);
                    }

                    _logger.Log(LogLevel.Debug, $"Report created: {reportFileName}");
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Debug, $"{dt}:  {ex.Message}  -- {ex.StackTrace}");
                }
            }
        }

    }
}
