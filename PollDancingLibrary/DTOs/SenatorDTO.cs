using System.Text.Json.Serialization;

namespace PollDancingLibrary.DTOs
{
    public class SenatorDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("bioguideId")]
        public string BioguideId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("partyName")]
        public string PartyName { get; set; }

        [JsonPropertyName("updateDate")]
        public string UpdateDate { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }
    }
}
