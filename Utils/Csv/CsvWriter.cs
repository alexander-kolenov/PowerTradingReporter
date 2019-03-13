using System.IO;
using System.Linq;

namespace Utils.Csv
{
    public class CsvWriter
    {

        public void Write(string path, CsvData data)
        {
            using (TextWriter tw = new StreamWriter(path))
            {

                tw.WriteLine(CreateLine(data.Headers));
                foreach (var x in data.Rows)
                {
                    tw.WriteLine(CreateLine(x));
                }

                tw.Flush();
            }
        }

        private string CreateLine(string[] values)
        {
            return string.Join(",", values.Select(x => x.IndexOfAny(new []{ '"', ',', '\r', '\n'}) < 0 ? x : $"\"{x.Replace("\"","\"\"")}\"")); // RFC4180
        }
    }
}
