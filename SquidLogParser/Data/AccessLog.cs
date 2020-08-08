using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SquidLogParser.Data
{
    public class AccessLog
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        
        public long Id { get; set; }

        [Column("time", TypeName = "timestamp")]
        public DateTime Time { get; set; }

        [Column("duration")]
        public long Duration { get; set; }

        [Column("client_address", TypeName = "varchar(200)")]
        public string ClientAddress { get; set; }

        [Column("result_code", TypeName = "varchar(150)")]
        public string ResultCode { get; set; }

        [Column("bytes")]
        public long Bytes { get; set; }

        [Column("request_method", TypeName = "varchar(50)")]
        public string RequestMethod { get; set; }

        [Column("url", TypeName = "varchar(500)")]
        public string Url { get; set; }

        [Column("user", TypeName = "varchar(50)")]
        public string User { get; set; }

        [Column("peer", TypeName = "varchar(100)")]
        public string Peer { get; set; }

        [Column("type", TypeName = "varchar(100)")]
        public string Type { get; set; }
    }
}