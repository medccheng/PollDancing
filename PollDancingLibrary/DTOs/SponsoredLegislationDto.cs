using System.Text.Json.Serialization;

namespace PollDancingLibrary.DTOs
{
    public class SponsoredLegislationDto
    {
        [JsonPropertyName("number")]
        public string Number { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("introducedDate")]
        public string IntroducedDate { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; } = null;

        [JsonPropertyName("summaries")]
        public string Summaries { get; set; }
    }
}