using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SquidLogParser.Data
{
    [Table("log_process_history")]
    public class LogProcessHistory
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("processed", TypeName = "timestamp")]
        public DateTime Processed { get; set; }

        [Column("lines_processed")]
        public long LinesProcessed { get; set; }        
    }
}