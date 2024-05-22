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

    public class MemberViewModel
    {
        public DepictionViewModel Depiction { get; set; }
        public AddressInformationViewModel AddressInformation { get; set; }
        public List<TermViewModel> Terms { get; set; }
        public string Name { get; set; }
        public string PartyName { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string BioguideId { get; set; }
        public string Url { get; set; }
        public DateTime UpdateDate { get; set; }

        public List<MemberLegislationVotesViewModel> MemberLegislationVotes { get; set; }

        public List<SponsoredLegislationViewModel> SponsoredLegislations { get; set; }

        public List<SponsoredLegislationViewModel> CosponsoredLegislations { get; set; }
    }

    public class DepictionViewModel
    {
        public int MemberId { get; set; }
        public string ImageUrl { get; set; }
        public string Attribution { get; set; }
    }

    public class AddressInformationViewModel
    {
        public int Id { get; set; }
        public string OfficeAddress { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public int MemberId { get; set; }
    }

    public class TermViewModel
    {
        public int Id { get; set; }
        public int StartYear { get; set; }
        public int? EndYear { get; set; } // Nullable for ongoing terms
        public string MemberType { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public int CongressId { get; set; }
        public CongressViewModel Congress { get; set; }
    }

    public class CongressViewModel
    {
        public int Id { get; set; }
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public List<SessionViewModel> Sessions { get; set; }
    }

    public class SessionViewModel
    {
        // Define session properties
    }
}
