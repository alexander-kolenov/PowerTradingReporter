using System;

namespace Reporter
{
    public class SessionInfo
    {
        /// <summary>
        /// This TimeSpan from 00:00 of previous calendar day
        /// 
        /// So if SessionStart is 18:00 and trading day is 2000-02-15
        /// That means that session starts 2000-02-14 18:00 UTC
        /// </summary>
        public TimeSpan SessionStart { get; set; }
    }
}
