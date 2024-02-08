using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace PollDancingLibrary.Models
{
    [Table("SponsoredLegislations")]
    public class SponsoredLegislation
    {

        [ForeignKey("Member")]
        public int MemberId { get; set; }

        public virtual Member Member { get; set; }


        [ForeignKey("Legislation")]
        public int LegislationId { get; set; }

        public virtual Legislation Legislation { get; set; }
    }

}