using System.Text.Json.Serialization;

namespace PollDancingLibrary.DTOs
{
    public class MemberLegislationVotesDto
    {
        [JsonPropertyName("legislationName")]
        
        public string? LegislationName { get; set; }

        [JsonPropertyName("vote")]
        public string? Vote { get; set; }
    }
}