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
        [JsonPropertyName("memberType")]
        public string? MemberType { get; set; }

        [StringLength(50)]
        [JsonPropertyName("stateName")]
        public string? StateName { get; set; }

        [StringLength(50)]
        [JsonPropertyName("stateCode")]
        public string? StateCode { get; set; }

        //[StringLength(50)]
        //[JsonPropertyName("congress")]
        //public int Congress { get; set; } = 0;






        [ForeignKey("Member")]
        public int MemberId { get; set; }

        public virtual Member Member { get; set; }


        [ForeignKey("Congress")]
        public int? CongressId { get; set; }

        public virtual Congress Congress { get; set; }

    }

}