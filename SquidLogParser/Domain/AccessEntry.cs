namespace SquidLogParser.Domain
{
    public class AccessEntry
    {
        public long Time { get; set; }
        public int Millis { get; set; }
        public long Elapsed { get; set; }
        public string RemoteHost { get; set; }
        public int Port { get; set; }
        public string Status { get; set; }
        public long Bytes { get; set; }
        public string Method { get; set; }
        public string Url { get; set; }
        public string User { get; set; }
        public string Peer { get; set; }
        public string Type { get; set; }

    }
}