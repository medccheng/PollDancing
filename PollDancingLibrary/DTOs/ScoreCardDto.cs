using System.Text.Json.Serialization;

namespace PollDancingLibrary.DTOs
{
    public class ScoreCardDTO
    {
        [JsonPropertyName("member")]
        public string Member { get; set; }

        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        [JsonPropertyName("score")]
        public string? Score { get; set; }

        [JsonPropertyName("comment")]
        public string? Comment { get; set; }

        [JsonPropertyName("focusArea")]
        public string? FocusArea { get; set; }

        [JsonPropertyName("relatedActions")]
        public string? RelatedActions { get; set; }
    }
}