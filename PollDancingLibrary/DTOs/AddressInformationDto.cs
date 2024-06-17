using System.Text.Json.Serialization;

namespace PollDancingLibrary.DTOs
{
    public class AddressInformationDto
    {
        [JsonPropertyName("street")]
        public string? Street { get; set; }

        [JsonPropertyName("city")]
        public string? City { get; set; }

        [JsonPropertyName("state")]
        public string? State { get; set; }

        [JsonPropertyName("district")]
        public string? District { get; set; }

        [JsonPropertyName("phone")]
        public string? Phone { get; set;}

        [JsonPropertyName("office")]
        public string? Office { get; set; }
    }
}