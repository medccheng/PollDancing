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
        //public LatestAction LatestAction { get; set; }
        public string? Number { get; set; } = null;
        //public PolicyArea PolicyArea { get; set; }
        public string? Title { get; set; } = null;
        public string? Type { get; set; } = null;
        public string? Url { get; set; } = null;


        public virtual ICollection<SponsoredLegislation> SponsoredLegislations { get; set; }

        public virtual ICollection<CosponsoredLegislation> CosponsoredLegislations { get; set; }

    }

}