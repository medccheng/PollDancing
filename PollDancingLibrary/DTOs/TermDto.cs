using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PollDancingLibrary.DTOs
{
    public class TermDto
    {
        [JsonPropertyName("startYear")]
        public int? StartYear { get; set; }

        [JsonPropertyName("endYear")]

        public int? EndYear { get; set; } // Optional for ongoing terms

        [JsonPropertyName("memberType")]
        public string? MemberType { get; set; }

        [JsonPropertyName("stateName")]
        public string? StateName { get; set; }

        [JsonPropertyName("stateCode")]
        public string? StateCode { get; set; }
    }
}