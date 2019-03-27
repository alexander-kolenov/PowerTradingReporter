using System;
using System.Globalization;

namespace Reporter
{
    public class ReportBuilder
    {
        public CsvData CreateCsvData(AggregatedData data)
        {
            CsvData report = new CsvData(new[] { "Local Time", "Volume" });

            DateTime utcSessionStart = data.TradingDate.AddDays(-1).Add(data.SessionStart);

            for (int i = 0; i < data.Volumes.Length; i++)
            {
                DateTime localTime = utcSessionStart.Add(TimeSpan.FromHours(i)).ToLocalTime();

                string time = localTime.TimeOfDay.ToString(@"hh\:mm");
                string volume = data.Volumes[i].ToString(CultureInfo.InvariantCulture);

                report.AddRow(new[] { time, volume });
            }
            return report;
        }

        public string GetCsvReportFileName(DateTime utcTime)
        {
            string str = utcTime.ToLocalTime().ToString("yyyyMMdd_hhmm");
            return $"PowerPosition_{str}.csv";
        }

    }
}
