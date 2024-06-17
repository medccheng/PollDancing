using System.Text.Json.Serialization;

namespace PollDancingScraper.JModels
{
    public class JCosponsoredLegislation
    {
        [JsonPropertyName("congress")]
        public int Congress { get; set; }

        [JsonPropertyName("introducedDate")]
        public DateTime IntroducedDate { get; set; }

        [JsonPropertyName("latestAction")]
        public LatestAction LatestAction { get; set; }

        [JsonPropertyName("number")]
        public string Number { get; set; }

        [JsonPropertyName("policyArea")]
        public PolicyArea PolicyArea { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}