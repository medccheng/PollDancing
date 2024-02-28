using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace PollDancingLibrary.Models
{
    [Table("Legislations")]
    public class Legislation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Congress { get; set; } = 0;
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



        #region one-to-many relationships
        public virtual ICollection<Action>? Actions { get; set; }

        //public virtual ICollection<Amendments>? Amendments { get; set; }

        //public virtual ICollection<CboCostEstimate>? CboCostEstimates { get; set; }

        //public ICollection<Summary> Summaries { get; set; }

        //public ICollection<TextVersion> TextVersions { get; set; }

        //public ICollection<Title>? Titles { get; set; }

        #endregion




        #region many-to-many relationships
        public virtual ICollection<SponsoredLegislation>? SponsoredLegislations { get; set; }

        public virtual ICollection<CosponsoredLegislation>? CosponsoredLegislations { get; set; }

        //public ICollection<Law>? Laws { get; set; }

        //public virtual ICollection<CommitteeReport>? CommitteeReports { get; set; }
        //public virtual ICollection<Committee>? Committees { get; set; }

        //public ICollection<RelatedLegislation>? RelatedBills { get; set; }

        //public ICollection<LegislativeSubject>? Subjects { get; set; }

        #endregion

        

    }

}