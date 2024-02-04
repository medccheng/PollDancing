namespace PollDancingWeb.Models
{
    public class MemberModel
    {
        public int Id { get; set; }
        public string BioguideId { get; set; }
        public string State { get; set; }
        public int? District { get; set; }
        public string? PartyName { get; set; }
        public string? Url { get; set; }
        //public DepictionModel? Depiction { get; set; }
        public string? UpdateDate { get; set; }
        public string Name { get; set; }
        //public List<TermModel> Terms { get; set; }
    }
}
