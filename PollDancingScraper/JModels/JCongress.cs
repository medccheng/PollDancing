using PollDancingLibrary.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PollDancingScraper.JModels
{
    public class JCongress
    {

        [JsonPropertyName("startYear")]
        public string? StartYear { get; set; }

        [JsonPropertyName("endYear")]

        public string? EndYear { get; set; } // Optional for ongoing terms

        [StringLength(50)]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("number")]
        public int Number { get; set; } = 0;



        public virtual List<JSession> Sessions { get; set; }
    }
}