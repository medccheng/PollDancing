using PollDancingLibrary.Migrations;
using System.Text.Json.Serialization;

namespace PollDancingScraper.JModels
{
    public class JAction
    {
        [JsonPropertyName("actionDate")]
        public DateTime ActionDate { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("actionCode")]
        public string ActionCode { get; set; }
        [JsonPropertyName("sourceSystem")]
        public JSourceSystem SourceSystem { get; set; }

        [JsonPropertyName("recordedVotes")]
        public IEnumerable<JRecordedVote> RecordedVotes { get; set; }
    }

    public class JRecordedVote
    {
        [JsonPropertyName("rollNumber")]
        public int RollNumber { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("chamber")]
        public string Chamber { get; set; }

        [JsonPropertyName("congress")]
        public int Congress { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("sessionNumber")]
        public int SessionNumber { get; set; }
    }

    public class JSourceSystem
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}