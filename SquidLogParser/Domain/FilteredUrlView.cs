using System.ComponentModel.DataAnnotations;

namespace SquidLogParser.Domain
{
    public class FilteredUrlView
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Url { get; set; }
    }
}