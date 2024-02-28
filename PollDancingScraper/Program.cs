using Microsoft.EntityFrameworkCore;
using PollDancingLibrary.Data;
using PollDancingLibrary.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using PollDancingScraper.JModels;
using static System.Net.WebRequestMethods;

class Program
{
    static readonly HttpClient client = new HttpClient();
    static readonly string apiKey = "ql4b4nm7Z9Mcgc29ryp4bCqAlYMuAOecwQe0gPTg";
    //static IConfigurationRoot Configuration;

    public static async Task Main(string[] args)
    {

        try
        {
            var optionsBuilder = new DbContextOptionsBuilder<CongressDbContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-LSNPVK6\\SQLEXPRESS;Database=CongressDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");

            using (var context = new CongressDbContext(optionsBuilder.Options))
            {
                Console.WriteLine("Saving all legislations data.");
                await SaveLegislations(context);

                foreach (var legislation in context.Legislations)
                {
                    Console.WriteLine(string.Format("Saving details for legislation {0}", legislation.Title));
                    await SaveLegislationDetails(context, legislation.Number, legislation.Congress, legislation.Type);
                }

                await context.SaveChangesAsync();
            }

            Console.WriteLine("All data saved.");
            Console.ReadLine();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
        }
    }

    private async static Task SaveLegislationDetails(CongressDbContext context, string? billNumber, int congressNumber, string? billType)
    {
        string uri = $"https://api.congress.gov/v3/bill/{congressNumber}/{billType}/{billNumber}?api_key={apiKey}";

        var response = await client.GetStringAsync(uri);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var root = JsonSerializer.Deserialize<JLegislationDetailsApiRoot>(response, options);

        var existingLegislation = await context.Legislations.FirstOrDefaultAsync(m => m.Number == billNumber);

        if (existingLegislation != null && root?.Bill != null)
        {
            var bill = root.Bill;
            //create a List of Term using the JTerm data from member.Terms
            foreach (var jmember in bill.Sponsors)
            {
                var member = context.Members.FirstOrDefault(m => m.BioguideId == jmember.BioguideId);
                if (member != null)
                {
                    var sponsoredLegislation = context.SponsoredLegislations.FirstOrDefault(t => t.LegislationId == existingLegislation.Id && t.MemberId == member.Id);
                    if (sponsoredLegislation == null)
                    {
                        context.SponsoredLegislations.Add(new SponsoredLegislation()
                        {
                            MemberId = member.Id
                            ,
                            LegislationId = existingLegislation.Id
                        });
                    }
                }                
            }
        }
    }

    private static async Task SaveLegislations(CongressDbContext context)
    {
        string uri = $"https://api.congress.gov/v3/bill?limit=250&api_key={apiKey}";
        var response = await client.GetStringAsync(uri);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var legislationsRoot = JsonSerializer.Deserialize<JLegislationApiRoot>(response, options);

        foreach (var bill in legislationsRoot.Bills)
        {
            var existingBill = await context.Legislations.FirstOrDefaultAsync(m => m.Number == bill.Number && m.Congress == bill.Congress);

            if (existingBill == null)
            {
                context.Legislations.Add(new Legislation()
                {
                    Number = bill.Number
                    ,
                    Congress = bill.Congress
                    ,
                    OriginChamber = bill.OriginChamber
                    ,
                    OriginChamberCode = bill.OriginChamberCode
                    ,
                    Title = bill.Title
                    ,
                    Type = bill.Type
                    ,
                    UpdateDate = bill.UpdateDate
                    ,
                    Url = bill.Url
                    ,
                    IntroducedDate = bill.IntroducedDate

                });
            }
            //else
            //{
            //    // Manually set each property you wish to update
            //    existingMember.District = member.District;
            //    existingMember.PartyName = member.PartyName;
            //    existingMember.State = member.State;
            //    existingMember.UpdateDate = member.UpdateDate;
            //    existingMember.Url = member.Url;
            //    context.Members.Update(existingMember);
            //}
        }
    }

    /// <summary>
    /// Save member data to the database
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private static async Task SaveMembers(CongressDbContext context)
    {
        string uri = $"https://api.congress.gov/v3/member?limit=250&api_key={apiKey}";
        var response = await client.GetStringAsync(uri);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var membersRoot = JsonSerializer.Deserialize<JMembersApiRoot>(response, options);

        foreach (var member in membersRoot.Members)
        {
            var existingMember = await context.Members.Include(m => m.Depiction)
                                                              .FirstOrDefaultAsync(m => m.BioguideId == member.BioguideId);

            if (existingMember == null)
            {
                context.Members.Add(new Member()
                {
                    BioguideId = member.BioguideId
                    ,
                    Name = member.Name
                    ,
                    District = member.District
                    ,
                    PartyName = member.PartyName
                    ,
                    State = member.State
                    ,
                    UpdateDate = member.UpdateDate
                    ,
                    Url = member.Url
                    ,
                    Depiction = member.Depiction,
                });
            }
            else
            {
                // Manually set each property you wish to update
                existingMember.District = member.District;
                existingMember.PartyName = member.PartyName;
                existingMember.State = member.State;
                existingMember.UpdateDate = member.UpdateDate;
                existingMember.Url = member.Url;
                context.Members.Update(existingMember);
            }
        }
    }


    private static async Task SaveMemberDetails(CongressDbContext context, string bioGuideId)
    {
        string uri = $"https://api.congress.gov/v3/member/{bioGuideId}?api_key={apiKey}";

        var response = await client.GetStringAsync(uri);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var root = JsonSerializer.Deserialize<JMemberDetailsApiRoot>(response, options);

        var existingMember = await context.Members.FirstOrDefaultAsync(m => m.BioguideId == bioGuideId);

        if (existingMember != null)
        {
            //create a List of Term using the JTerm data from member.Terms
            if (root.Member.Terms != null)
            {
                existingMember.Terms = context.Terms.Where(t => t.MemberId == existingMember.Id).ToList();

                foreach (var jterm in root.Member.Terms)
                {
                    if (!existingMember.Terms.Any(x => x.StartYear == jterm.StartYear && x.EndYear == jterm.EndYear))
                        existingMember.Terms.Add(new Term()
                        {
                            StartYear = jterm.StartYear
                        ,
                            EndYear = jterm.EndYear
                        ,
                            MemberType = jterm.MemberType
                        ,
                            StateName = jterm.StateName
                        ,
                            StateCode = jterm.StateCode
                        ,
                            Congress = jterm.Congress
                        ,
                            MemberId = existingMember.Id
                        });
                }
            }

            if (root.Member.AddressInformation != null)
            {
                existingMember.AddressInformation = new AddressInformation()
                {
                    OfficeAddress = root.Member.AddressInformation.OfficeAddress
                    ,
                    City = root.Member.AddressInformation.City
                    ,
                    District = root.Member.AddressInformation.District
                    ,
                    ZipCode = root.Member.AddressInformation.ZipCode
                    ,
                    PhoneNumber = root.Member.AddressInformation.PhoneNumber
                    ,
                    MemberId = existingMember.Id
                };
            }

            context.Members.Update(existingMember);
        }
    }

}


//public class JDepiction
//{

//    [JsonPropertyName("imageUrl")]
//    public string? ImageUrl { get; set; }

//    [JsonPropertyName("attribution")]
//    public string? Attribution { get; set; }

//    [ForeignKey("JMember")]
//    public int MemberId { get; set; }
//    public virtual JMember Member { get; set; }
//}

