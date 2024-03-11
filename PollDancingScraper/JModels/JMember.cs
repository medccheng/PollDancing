using PollDancingLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PollDancingScraper.JModels
{
    public class JMember
    {
        [JsonPropertyName("bioguideId")]
        public string BioguideId { get; set; }

        [JsonPropertyName("state")]
        public string? State { get; set; }

        [JsonPropertyName("district")]
        public int? District { get; set; } // Optional for senators

        [JsonPropertyName("partyName")]
        public string? PartyName { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("depiction")]
        public virtual Depiction? Depiction { get; set; }

        [JsonPropertyName("updateDate")]
        public DateTime? UpdateDate { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }


        //[JsonPropertyName("terms")]
        //public List<Term> Terms { get; set; }

        //[JsonPropertyName("addressInformation")]
        //public AddressInformation AddressInformation { get; set; }
    }
}
