using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace PollDancingLibrary.Models
{
    [Table("ScoreCards")]
    public class ScoreCard : BaseClass
    {

        [ForeignKey("Member")]
        public int MemberId { get; set; }

        public virtual Member Member { get; set; }


        [ForeignKey("Subject")]
        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; }

        [StringLength(100)]
        public string? Score { get; set; }

        [ForeignKey("Comment")]
        [Column(TypeName = "nvarchar(max)")]
        public string? Comment { get; set; }

        [ForeignKey("FocusArea")]
        [Column(TypeName = "nvarchar(max)")]
        public string? FocusArea { get; set; }

        [ForeignKey("RelatedActions")]
        [Column(TypeName = "nvarchar(max)")]
        public string? RelatedActions { get; set; }
    }

}