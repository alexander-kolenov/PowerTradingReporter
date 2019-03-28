using Csv;
using NLog;
using Quartz;
using System;
using System.IO;
using System.Threading.Tasks;
using Unity;

namespace Reporter
{
    public class MakeReportJob : IJob
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [Dependency]
        public TradingReporterConfiguration Config { get; set; }


        public async Task Execute(IJobExecutionContext context = null)
        {
            Config = (TradingReporterConfiguration)context?.MergedJobDataMap[nameof(TradingReporterConfiguration)];
            DateTime utcTime = DateTime.UtcNow;
            DataAcquisition da = new DataAcquisition();
            DateTime tradingDate = da.GetTradingDay(utcTime, Config.SessionInfo);
            AggregatedData ad = await da.GetAggregatedTradesAsync(tradingDate, Config.SessionInfo);

            ReportBuilder rb = new ReportBuilder();
            Directory.CreateDirectory(Config.ReportingDirrectory);
            string reportFileName = Path.Combine(Config.ReportingDirrectory, rb.GetCsvReportFileName(utcTime));
            CsvData csvData = rb.CreateCsvData(ad);

            using (TextWriter tw = new StreamWriter(reportFileName))
                CsvWriter.Write(tw, csvData.Headers, csvData.Rows);

            Logger.Log(LogLevel.Debug, $"Report created: {reportFileName}");
        }
    }
}
