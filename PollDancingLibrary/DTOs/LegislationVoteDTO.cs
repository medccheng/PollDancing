using System.Text.Json.Serialization;

namespace PollDancingLibrary.DTOs
{
    public class LegislationVoteDTO
    {

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("vote")]
        public string? Vote { get; set; }

        //[JsonPropertyName("yes")]
        //public string? Yes { get; set; }

        //[JsonPropertyName("no")]
        //public string? No { get; set; }

        //[JsonPropertyName("notVoting")]
        //public string? NotVoting { get; set; }
    }
}