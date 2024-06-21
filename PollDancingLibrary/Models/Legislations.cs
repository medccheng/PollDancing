using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace PollDancingLibrary.Models
{
    [Table("Legislations")]
    public class Legislation: BaseClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime? IntroducedDate { get; set; } = null;
        public string? Number { get; set; } = null;
        public string? Title { get; set; } = null;
        public string? Type { get; set; } = null;
        public string? Url { get; set; } = null;
        public string? ConstitutionalAuthorityStatementText { get; set; }
        public string? OriginChamber { get; set; }

        public string? OriginChamberCode { get; set; } 
        public DateTime? UpdateDate { get; set; }
        public DateTime? UpdateDateIncludingText { get; set; }

        [ForeignKey("PolicyArea")]
        public int? PolicyAreaId { get; set; }
        public virtual PolicyArea? PolicyArea { get; set; }

        public string? Summaries { get; set; }

        //Actual ID of Congress not the DB ID
        public int? CongressId { get; set; }

        public bool? NeedsUpdate { get; set; } = false;


        #region one-to-many relationships
        public virtual ICollection<Action>? Actions { get; set; }

        #endregion


        #region many-to-many relationships
        public virtual ICollection<SponsoredLegislation>? SponsoredLegislations { get; set; }

        public virtual ICollection<CosponsoredLegislation>? CosponsoredLegislations { get; set; }

        public virtual ICollection<MemberLegislationVotes>? MemberLegislationVotes { get; set; }

        #endregion

    }

}