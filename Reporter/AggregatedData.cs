using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporter
{
    public class AggregatedData
    {
        public DateTime Date { get; set; }
        public double[] Volumes { get; set; } = new double[24];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="period">range [1;24]</param>
        /// <param name="volume"></param>
        internal void Add(int period, double volume)
        {
            period -= 1; 
            if (period < 0 || period >= Volumes.Length) throw new ArgumentException("period", "Unexpected period");
            Volumes[period] += volume;
        }
    }
}
