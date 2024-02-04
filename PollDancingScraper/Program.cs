using Microsoft.EntityFrameworkCore;
using PollDancingLibrary.Data;
using PollDancingLibrary.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

class Program
{
    static readonly HttpClient client = new HttpClient();
    //static IConfigurationRoot Configuration;

    public static async Task Main(string[] args)
    {

        try
        {
            string uri = "https://api.congress.gov/v3/member?limit=1000&api_key=ql4b4nm7Z9Mcgc29ryp4bCqAlYMuAOecwQe0gPTg";
            var response = await client.GetStringAsync(uri);

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var membersRoot = JsonSerializer.Deserialize<JApiRoot>(response, options);

            var optionsBuilder = new DbContextOptionsBuilder<CongressDbContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-LSNPVK6\\SQLEXPRESS;Database=CongressDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");

            using (var context = new CongressDbContext(optionsBuilder.Options))
            {
                foreach (var member in membersRoot.Members)
                {
                    var existingMember = await context.Members.Include(m => m.Depiction)
                                                              .FirstOrDefaultAsync(m => m.BioguideId == member.BioguideId);
                    var terms = new List<Term>();


                    if (existingMember == null)
                    {
                        if (member.Terms != null && member.Terms.Item != null)
                        {
                            foreach (var t in member.Terms.Item)
                            {
                                terms.Add(new Term() { StartYear = t.StartYear, EndYear = t.EndYear, Chamber = t.Chamber });
                            }
                        }
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
                            Depiction = member.Depiction
                            ,
                            Terms = terms
                        });
                    }
                    else
                    {
                        if (member.Terms != null && member.Terms.Item != null)
                        {
                            foreach (var t in member.Terms.Item)
                            {
                                terms.Add(new Term() { StartYear = t.StartYear, EndYear = t.EndYear, Chamber = t.Chamber, MemberId = existingMember.Id });
                            }
                        }
                        // Manually set each property you wish to update
                        existingMember.District = member.District;
                        existingMember.PartyName = member.PartyName;
                        existingMember.State = member.State;
                        existingMember.UpdateDate = member.UpdateDate;
                        existingMember.Url = member.Url;

                        // Assuming Depiction is a complex type and you want to replace it entirely
                        // Make sure to check for nulls or initialize new instances as needed
                        existingMember.Depiction = member.Depiction; // This might need custom logic if you need to update fields individually

                        // For updating collections like Terms, you might need more logic,
                        // especially if you need to add new terms, update existing ones, or remove old ones
                        // This example assumes you want to replace the entire collection, which might not be what you want
                        if (existingMember.Terms != null)
                            existingMember.Terms.Clear();

                        existingMember.Terms = terms;

                        context.Members.Update(existingMember);
                    }
                }

                await context.SaveChangesAsync();
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
        }
    }
}


public class JApiRoot
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

    [JsonPropertyName("terms")]
    public JItem Terms { get; set; }

}


public class JItem
{
    [JsonPropertyName("item")]
    public virtual List<JTerm> Item { get; set; }

}

public class JTerm
{

    [JsonPropertyName("startYear")]
    public int? StartYear { get; set; }

    [JsonPropertyName("endYear")]

    public int? EndYear { get; set; } // Optional for ongoing terms

    [StringLength(50)]
    [JsonPropertyName("chamber")]
    public string? Chamber { get; set; }

    [ForeignKey("JMember")]
    public int MemberId { get; set; }

    public virtual JMember Member { get; set; }

}


public class JDepiction
{

    [JsonPropertyName("imageUrl")]
    public string? ImageUrl { get; set; }

    [JsonPropertyName("attribution")]
    public string? Attribution { get; set; }

    [ForeignKey("JMember")]
    public int MemberId { get; set; }
    public virtual JMember Member { get; set; }
}