using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PollDancingScraper.JModels
{

    public class JAddressInformation
    {
        [JsonPropertyName("officeAddress")]
        public string OfficeAddress { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("district")]
        public string District { get; set; }

        [JsonPropertyName("zipCode")]
        public int ZipCode { get; set; }

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }


        //[ForeignKey("JMember")]
        //public int MemberId { get; set; }
        //public virtual JMember Member { get; set; }
    }
}
