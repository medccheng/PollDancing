using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace PollDancingLibrary.Models
{
    [Table("Depictions")]
    public class Depiction: BaseClass
    {
        [Key]
        [ForeignKey("Member")]
        public int MemberId { get; set; }

        [Required]
        [StringLength(200)]
        [JsonPropertyName("imageUrl")]
        public string? ImageUrl { get; set; }

        [Required]
        [JsonPropertyName("attribution")]
        public string? Attribution { get; set; }

        public virtual Member Member { get; set; }
    }
}
