using PollDancingLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PollDancingScraper.JModels
{
    public class JLegislation
    {
        [JsonPropertyName("congress")]
        public int Congress { get; set; }

        [JsonPropertyName("number")]
        public string? Number { get; set; }

        [JsonPropertyName("originChamber")]
        public string? OriginChamber { get; set; }

        [JsonPropertyName("originChamberCode")]
        public string? OriginChamberCode { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("updateDate")]
        public DateTime? UpdateDate { get; set; }        

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("introducedDate")]
        public DateTime? IntroducedDate { get; set; }

        [JsonPropertyName("sponsors")]
        public List<JMember> Sponsors { get; set; }
    }    
}
