using System;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Csv;

namespace UnitTests
{
    [TestClass]
    public class CsvUnitTest
    {

        public string GetCurrentMethod([CallerMemberName] string name = "name")
        {
            return Path.Combine(GetType().Name, name);
        }

        [TestMethod]
        public void TestMethod1()
        {
            string fileName = $"{GetCurrentMethod()}.csv";

            CsvData data = new CsvData(new[] { "head1", "head2" });
            data.AddRow(new[] { ",", "comma"});
            data.AddRow(new[] { "\"", "quote" });
            data.AddRow(new[] { "\r", "r" });
            data.AddRow(new[] { "\n", "n" });

            var csvWriter = new CsvWriter();
            csvWriter.Write(fileName, data);

            using (TextReader tr = new StreamReader(fileName))
            {
                string str = tr.ReadToEnd();
                string result =
                    "head1,head2" + Environment.NewLine +
                    "\",\",comma" + Environment.NewLine +
                    "\"\"\"\",quote" + Environment.NewLine +
                    "\"\r\",r" + Environment.NewLine +
                    "\"\n\",n" + Environment.NewLine;

                Assert.AreEqual(str, result);
            }
        }
    }
}
