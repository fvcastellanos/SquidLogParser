namespace SquidLogParser.Domain
{
    public class AccessEntry
    {
        public double Time { get; set; }
        public double Elapsed { get; set; }
        public string RemoteHost { get; set; }
        public int Port { get; set; }
        public string Status { get; set; }
        public long Bytes { get; set; }
        public string Method { get; set; }
        public string Url { get; set; }
        public string PeerHost { get; set; }
        public string Type { get; set; }

    }
}