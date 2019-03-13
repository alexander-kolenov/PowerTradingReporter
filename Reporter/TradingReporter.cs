using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Csv;

namespace Reporter
{
    public class TradingReporter
    {
        public TradingReporterConfiguration Config { get; set; }

        public TradingReporter(TradingReporterConfiguration config)
        {
            Config = config;
        }

        public void MakeReport(DateTime localTime)
        {
            DataAcquisition da = new DataAcquisition();
            AggregatedData ad = da.GetAggregatedTrades(localTime);

            ReportBuilder rb = new ReportBuilder();
            string reportFileName = Path.Combine(Config.ReportingDirrectory, rb.GetCsvReportFileName(localTime));
            CsvData csvData = rb.CreateCsvData(ad);

            CsvWriter w = new CsvWriter();
            w.Write(reportFileName, csvData);
        }

        public void Start()
        {

        }
        public void Stop()
        {

        }

    }
}
