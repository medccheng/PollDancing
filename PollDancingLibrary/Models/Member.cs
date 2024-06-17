using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PollDancingLibrary.Models
{
    [Table("Members")]
    public class Member
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        [JsonPropertyName("bioguideId")]
        public string? BioguideId { get; set; }

        [Required]
        [StringLength(100)]
        [JsonPropertyName("state")]
        public string? State { get; set; }

        [JsonPropertyName("district")]
        public int? District { get; set; } // Optional for senators

        [Required]
        [StringLength(50)]
        [JsonPropertyName("partyName")]
        public string? PartyName { get; set; }

        [Required]
        [StringLength(200)]
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("depiction")]
        public virtual Depiction? Depiction { get; set; }

        [Required]
        [JsonPropertyName("updateDate")]
        public DateTime? UpdateDate { get; set; }

        [Required]
        [StringLength(200)]
        [JsonPropertyName("name")]
        public string? Name { get; set; }


        [ForeignKey("AddressInformation")]
        public int? AddressInformationId { get; set; }

        [JsonPropertyName("addressInformation")]
        public virtual AddressInformation? AddressInformation { get; set; }

        public bool IsCurrent { get; set; } = false;



        [JsonPropertyName("terms")]
        public virtual ICollection<Term> Terms { get; set; }        

        public virtual ICollection<SponsoredLegislation> SponsoredLegislations { get; set; }

        public virtual ICollection<CosponsoredLegislation> CosponsoredLegislations { get; set; }

        public virtual ICollection<MemberLegislationVotes> MemberLegislationVotes { get; set; }
    }
}
