using System;

namespace Ap.IISLogger
{
    public class IisLogCount
    {
        public int Hit { get; set; }
        public DateTime EntryTime { get; set; }
        public string Uri { get; set; }
        public string Method { get; set; }
    }
}
