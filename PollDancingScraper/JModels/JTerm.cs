using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PollDancingScraper.JModels
{
    public class JTerm
    {

        [JsonPropertyName("startYear")]
        public int? StartYear { get; set; }

        [JsonPropertyName("endYear")]

        public int? EndYear { get; set; } // Optional for ongoing terms

        [StringLength(50)]
        [JsonPropertyName("memberType")]
        public string? MemberType { get; set; }

        [StringLength(50)]
        [JsonPropertyName("stateName")]
        public string? StateName { get; set; }

        [StringLength(50)]
        [JsonPropertyName("stateCode")]
        public string? StateCode { get; set; }

        [JsonPropertyName("congress")]
        public int Congress { get; set; }



        ////[ForeignKey("JMember")]
        ////public int MemberId { get; set; }

        //public virtual JCongress Congress { get; set; }
    }
}
