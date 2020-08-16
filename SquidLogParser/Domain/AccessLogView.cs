using System;

namespace SquidLogParser.Domain
{
    public class AccessLogView
    {
        public string Url { get; set; }
        public DateTime Time { get; set; }    
        public long Duration { get; set; }
        public string ClientAddress { get; set; }
        public long Bytes { get; set; }
        public string RequestMethod { get; set; }
        public string Peer { get; set; }
        public string Type { get; set; }
        public string ResultCode { get; set; }
        public int Count { get; set; }
    }
}