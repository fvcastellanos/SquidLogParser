using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SquidLogParser.Data
{
    [Table("filtered_url")]
    public class FilteredUrl
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("url", TypeName = "varchar(250)")]
        public string Url { get; set; }

        [Column("created", TypeName = "timestamp")]
        public DateTime Created { get; set; }
    }
}