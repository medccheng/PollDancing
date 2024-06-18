using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace PollDancingLibrary.Models
{
    [Table("Congresses")]
    public class Congress : BaseClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [JsonPropertyName("startYear")]
        public string? StartYear { get; set; }

        [JsonPropertyName("endYear")] 

        public string? EndYear { get; set; } // Optional for ongoing terms


        [StringLength(50)]
        [JsonPropertyName("number")]
        public int Number { get; set; } = 0;

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        public bool IsCurrent { get; set; }


        public virtual List<Session> Sessions { get; set; }

    }

}