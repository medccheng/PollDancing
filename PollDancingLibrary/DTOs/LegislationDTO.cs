using System.Text.Json.Serialization;

namespace PollDancingLibrary.DTOs
{
    public class LegislationDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("number")]
        public string? Number { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("summaries")]
        public string? Summaries { get; set; }
      

    }
}