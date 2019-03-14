using System;

namespace Reporter
{
    public class AggregatedData
    {
        /// <summary>
        /// Trading seession date
        /// </summary>
        public DateTime TradingDate { get; set; }


        /// <summary>
        /// This TimeSpan from 00:00 of previous calendar day
        /// 
        /// So if SessionStart is 18:00 and trading day is 2000-02-15
        /// That means that session starts 2000-02-14 18:00 UTC
        /// </summary>
        public TimeSpan SessionStart { get; set; }
        
        
        /// <summary>
        /// Array of 24 volumes per hour since trading session starts
        /// </summary>
        public double[] Volumes { get; set; } = new double[24];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="period">range [1;24]</param>
        /// <param name="volume"></param>
        internal void AddVolumeToPaticularPeriod(int period, double volume)
        {
            period -= 1; 
            if (period < 0 || period >= Volumes.Length) throw new ArgumentException("period", "Unexpected period");
            Volumes[period] += volume;
        }
    }
}
