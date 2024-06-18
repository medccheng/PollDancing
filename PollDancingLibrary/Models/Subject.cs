using System.ComponentModel.DataAnnotations.Schema;

namespace PollDancingLibrary.Models
{
    [Table("Subjects")]
    public class Subject : BaseClass
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<ScoreCard> ScoreCards { get; set; }
    }
}