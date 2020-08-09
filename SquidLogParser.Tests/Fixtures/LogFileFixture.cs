using System.Collections.Generic;
using System.IO;
using System.Linq;
using SquidLogParser.Domain;

namespace SquidLogParser.Tests.Fixtures
{
    public static class LogFileFixture
    {
        public static IEnumerable<string> ReadLogs()
        {
            return new List<string>()
            {
                "1596340895.047      7 192.168.0.132 TAG_NONE/503 0 CONNECT mobile.pipe.aria.microsoft.com:443 - HIER_NONE/- -",
                "1596340901.441    728 192.168.0.132 TCP_TUNNEL/200 8664 CONNECT nav.smartscreen.microsoft.com:443 - HIER_DIRECT/13.88.23.8 -",
                "1596340902.296    239 192.168.0.132 TCP_TUNNEL/200 6150 CONNECT edge.activity.windows.com:443 - HIER_DIRECT/20.36.219.28 -",
                "1596340902.494     12 192.168.0.132 TAG_NONE/503 0 CONNECT c.msn.com:443 - HIER_NONE/- -",
                "1596340902.499     15 192.168.0.132 TAG_NONE/503 0 CONNECT otf.msn.com:443 - HIER_NONE/- -"
            };
        }

        public static string WriteLogs()
        {            
            var tempFile = Path.GetTempFileName();
            File.WriteAllLines(tempFile, ReadLogs());

            return tempFile;
        }

        public static void DeleteTempLogs(string fileName)
        {
            File.Delete(fileName);
        }

        public static IEnumerable<AccessEntry> GetAccessEntries()
        {
            return GetAccessEntries(ReadLogs());
        }

        public static IEnumerable<AccessEntry> GetAccessEntries(IEnumerable<string> logs)
        {
            return logs.Select(ConvertLogentry)
                .ToList();
        }

        // ---------------------------------------------------------------------------------------

        private static AccessEntry ConvertLogentry(string logEntry)
        {
            var fields = logEntry.Split(" ").ToList()
                .Where(text => !string.IsNullOrEmpty(text))
                .ToArray();

            var time = fields[0].Substring(0, 10);
            var separatorPosition = fields[0].LastIndexOf(".");
            var millis = fields[0].Substring(separatorPosition + 1);

            return new AccessEntry()
            {
                Time = long.Parse(time),
                Elapsed = int.Parse(millis),
                RemoteHost = fields[2],
                Status = fields[3],
                Bytes = long.Parse(fields[4]),
                Method = fields[5],
                Url = fields[6],
                User = fields[7],
                Peer = fields[8],
                Type = fields[9]
            };
        }
    }
}