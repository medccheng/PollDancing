using Microsoft.EntityFrameworkCore;
using PollDancingLibrary.Data;
using PollDancingLibrary.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

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
                Console.WriteLine("Saving all members data.");
                await SaveMember(context);

                //int i = 0;
                foreach (var member in context.Members)
                {
                    Console.WriteLine(string.Format("Saving details for member {0}", member.Name));
                    await SaveMemberDetails(context, member.BioguideId);
                    //i++;
                    //if (i > 5) break;
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

    /// <summary>
    /// Save member data to the database
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private static async Task SaveMember(CongressDbContext context)
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







public class JMembersApiRoot
{
    [JsonPropertyName("members")]
    public List<JMember> Members { get; set; }
}


public class JMember
{
    [JsonPropertyName("bioguideId")]
    public string BioguideId { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("district")]
    public int? District { get; set; } // Optional for senators

    [JsonPropertyName("partyName")]
    public string? PartyName { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("depiction")]
    public virtual Depiction? Depiction { get; set; }

    [JsonPropertyName("updateDate")]
    public DateTime? UpdateDate { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }


    //[JsonPropertyName("terms")]
    //public List<Term> Terms { get; set; }

    //[JsonPropertyName("addressInformation")]
    //public AddressInformation AddressInformation { get; set; }
}

public class JMemberDetailsApiRoot
{
    [JsonPropertyName("member")]
    public JMemberDetails Member { get; set; }
}

public class JMemberDetails
{
    [JsonPropertyName("bioguideId")]
    public string BioguideId { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("district")]
    public int? District { get; set; } // Optional for senators

    [JsonPropertyName("partyName")]
    public string? PartyName { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("depiction")]
    public virtual Depiction? Depiction { get; set; }

    [JsonPropertyName("updateDate")]
    public DateTime? UpdateDate { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("terms")]
    public List<JTerm> Terms { get; set; }

    [JsonPropertyName("addressInformation")]
    public JAddressInformation AddressInformation { get; set; }
}



public class JTerm
{

    [JsonPropertyName("startYear")]
    public int? StartYear { get; set; }

    [JsonPropertyName("endYear")]

    public int? EndYear { get; set; } // Optional for ongoing terms

    [StringLength(50)]
    [JsonPropertyName("memberType")]
    public string? MemberType { get; set; }

    [StringLength(50)]
    [JsonPropertyName("stateName")]
    public string? StateName { get; set; }

    [StringLength(50)]
    [JsonPropertyName("stateCode")]
    public string? StateCode { get; set; }

    [JsonPropertyName("congress")]
    public int Congress { get; set; }

    [ForeignKey("JMember")]
    public int MemberId { get; set; }

    //public virtual JMember Member { get; set; }
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


public class JAddressInformation
{
    [JsonPropertyName("officeAddress")]
    public string OfficeAddress { get; set; }

    [JsonPropertyName("city")]
    public string City { get; set; }

    [JsonPropertyName("district")]
    public string District { get; set; }

    [JsonPropertyName("zipCode")]
    public int ZipCode { get; set; }

    [JsonPropertyName("phoneNumber")]
    public string PhoneNumber { get; set; }


    //[ForeignKey("JMember")]
    //public int MemberId { get; set; }
    //public virtual JMember Member { get; set; }
}