using System;
using System.Globalization;
using Utils.Csv;

namespace Reporter
{
    public class ReportBuilder
    {
        public CsvData CreateCsvData(AggregatedData data)
        {
            CsvData report = new CsvData(new[] { "Local Time", "Volume" });

            for (int i = 0; i < data.Volumes.Length; i++)
            {
                string time = TimeSpan.FromHours(i + 23).ToString(@"hh\:mm");
                string volume = data.Volumes[i].ToString(CultureInfo.InvariantCulture);

                report.AddRow(new[] { time, volume });
            }
            return report;
        }

        public string GetCsvReportFileName(DateTime extractionTime)
        {
            string str = extractionTime.ToString("yyyyMMdd_hhmm");
            return $"PowerPosition_{str}.csv";
        }

    }
}
