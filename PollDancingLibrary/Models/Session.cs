using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PollDancingLibrary.Models
{
    [Table("Sessions")]
    public class Session
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [JsonPropertyName("startYear")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("endYear")]

        public DateTime? EndDate { get; set; } // Optional for ongoing terms


        [StringLength(50)]
        [JsonPropertyName("number")]
        public int Number { get; set; } = 0;


        [StringLength(50)]
        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }
}