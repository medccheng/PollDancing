using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace PollDancingLibrary.Models
{
    [Table("Terms")]
    public class Term
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [JsonPropertyName("startYear")]
        public int? StartYear { get; set; }

        [JsonPropertyName("endYear")] 

        public int? EndYear { get; set; } // Optional for ongoing terms

        [StringLength(50)]
        [JsonPropertyName("chamber")]
        public string? Chamber { get; set; }

        [ForeignKey("Member")]
        public int MemberId { get; set; }

        public virtual Member Member { get; set; }

    }

}