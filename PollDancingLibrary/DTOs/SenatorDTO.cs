using PollDancingLibrary.Models;
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


        [JsonPropertyName("addressInformation")]
        public AddressInformationDto AddressInformation { get; set; }


        //[JsonPropertyName("sponsoredLegislation")]
        //public List<SponsoredLegislationDto> SponsoredLegislations { get; set; }


        //[JsonPropertyName("cosponsoredLegislation")]
        //public List<SponsoredLegislationDto> CosponsoredLegislations { get; set; }


        [JsonPropertyName("terms")]
        public List<TermDto> Terms { get; set; }


        //[JsonPropertyName("legislationVotes")]
        //public List<MemberLegislationVotesDto> MemberLegislationVotes { get; set; }

        //[JsonPropertyName("scoreCards")]
        //public List<ScoreCardDto> ScoreCards { get; set; }

    }
}
