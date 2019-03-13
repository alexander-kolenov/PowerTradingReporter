using System.Collections.Generic;

namespace Utils.Csv
{
    public class CsvData
    {
        public string[] Headers { get; set; }
        public List<string[]> Rows { get; set; }


        public CsvData(string[] headers)
        {
            Headers = headers;
            Rows = new List<string[]>();
        }

        public void AddRow(string[] row)
        {
            Rows.Add(row);
        }
    }
}
