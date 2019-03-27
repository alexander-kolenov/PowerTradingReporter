using Csv;
using NLog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Reporter
{
    public class ReportProcessor
    {
        public TradingReporterConfiguration Config { get; set; }

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public ReportProcessor(TradingReporterConfiguration config)
        {
            Config = config;

        }

        public Task MakeReportTask() => new Task(() =>
        {
            DateTime utcTime = DateTime.UtcNow;
            DataAcquisition da = new DataAcquisition();
            DateTime tradingDate = da.GetTradingDay(utcTime, Config.SessionInfo);
            AggregatedData ad = da.GetAggregatedTrades(tradingDate, Config.SessionInfo);

            ReportBuilder rb = new ReportBuilder();
            Directory.CreateDirectory(Config.ReportingDirrectory);
            string reportFileName = Path.Combine(Config.ReportingDirrectory, rb.GetCsvReportFileName(utcTime));
            CsvData csvData = rb.CreateCsvData(ad);

            using (TextWriter tw = new StreamWriter(reportFileName))
            {
                CsvWriter.Write(tw, csvData.Headers, csvData.Rows);
            }

            _logger.Log(LogLevel.Debug, $"Report created: {reportFileName}");
        });
    }
}
