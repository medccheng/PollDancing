using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace PollDancingLibrary.Models
{
    [Table("AddressInformations")]

    public class AddressInformation: BaseClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? OfficeAddress { get; set; }

        public string? City { get; set; }

        public string? District { get; set; }

        public int ZipCode { get; set; } = 0;

        public string? PhoneNumber { get; set; }




        [ForeignKey("Member")]
        public int MemberId { get; set; }

        public virtual Member Member { get; set; }

    }

}