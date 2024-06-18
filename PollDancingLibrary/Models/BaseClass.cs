using System.ComponentModel.DataAnnotations.Schema;

namespace PollDancingLibrary.Models
{
    public class BaseClass
    {
        public DateTime? CreatedDate { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? CreatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? UpdatedBy { get; set; }
    }
}