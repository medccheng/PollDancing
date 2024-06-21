using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Diagnostics.Eventing.Reader;

namespace PollDancingLibrary.Models
{
    [Table("Actions")]
    public class Action: BaseClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [JsonPropertyName("actionCode")]
        public string? ActionCode { get; set; }

        [JsonPropertyName("actionDate")]
        public DateTime ActionDate { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("recordedVotes")]
        public string? RecordedVotes { get; set; }

       
        //public string? RecordedVotesUrl { get; set; }

        [ForeignKey("Legislation")]
        public int? LegislationId { get; set; }

        public virtual Legislation? Legislation { get; set; }

        public bool IsParsed { get; set; }

        //public SourceSystem SourceSystem { get; set; }
    }
}
