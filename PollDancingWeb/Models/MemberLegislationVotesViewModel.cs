namespace PollDancingWeb.Models
{
    public class MemberLegislationVotesViewModel
    {
        public int MemberId { get; set; }
        public int LegislationId { get; set; }
        public string Vote { get; set; }

        public string LegislationTitle { get; set; }
    }
}