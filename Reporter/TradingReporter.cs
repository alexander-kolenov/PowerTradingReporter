using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

        private Timer _timer;
        private Task _currentTask;


        public void OnStart()
        {
            if (_timer == null)
            {
                _timer = new Timer(new TimerCallback(TimerProc), null, 0, Config.ReportingIntervalInMinutes * 1000);
            }
            else
            {
                _timer.Change(0, Config.ReportingIntervalInMinutes * 1000);
            }
        }

        private void TimerProc(object state)
        {
            _currentTask = Task.Run(() => MakeReport(DateTime.Now));
        }

        public void OnStop()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            if (_currentTask != null)
            {
                Task.WaitAll(new[] { _currentTask });
            }
        }


        public void OnContinue() => _timer.Change(0, Config.ReportingIntervalInMinutes * 1000);
        public void OnPause() => _timer.Change(Timeout.Infinite, Timeout.Infinite);
    }
}
