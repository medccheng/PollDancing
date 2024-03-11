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
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;

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

                //await SaveCongresses(context);

                //await SaveMembers(context);

                //int i = 0;
                foreach (var member in context.Members
    .Include(m => m.Terms)
    .Where(m => !m.Terms.Any() && m.UpdateDate > DateTime.Parse("1/1/2023"))
    .Take(200))
                {
                    await SaveMemberDetails(context, member.BioguideId);
                }

                //Console.WriteLine("Saving all legislations data.");
                //await SaveLegislations(context);

                //foreach (var legislation in context.Legislations)
                //{
                //    //Console.WriteLine(string.Format("Saving details for legislation {0}", legislation.Title));
                //    await SaveLegislationDetails(context, legislation.Number, legislation.Congress, legislation.Type);
                //}

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

    private static async Task SaveCongresses(CongressDbContext context)
    {
        Console.WriteLine("Saving all congresses list.");
        string uri = $"https://api.congress.gov/v3/congress?limit=250&api_key={apiKey}";
        var response = await client.GetStringAsync(uri);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var congressesRoot = JsonSerializer.Deserialize<JCongressesApiRoot>(response, options);

        foreach (var congress in congressesRoot.Congresses)
        {
            string pattern = @"\d+";
            Match match = Regex.Match(congress.Name, pattern);
            int id = 0;
            var existingCongress = await context.Congresses.FirstOrDefaultAsync(c => c.Number == (int.TryParse(match.Value, out id) ? id : 0));

            if (existingCongress == null)
            {
                context.Congresses.Add(new Congress()
                {
                    Name = congress.Name
                    ,
                    StartYear = congress.StartYear
                    ,
                    EndYear = congress.EndYear
                    ,
                    Number = (int.TryParse(match.Value, out id) ? id : 0)
                });
            }
            else
            {
                //existingCongress.StartYear = congress.StartYear;
                //existingCongress.EndYear = congress.EndYear;
                existingCongress.Number = (int.TryParse(match.Value, out id) ? id : 0);
                existingCongress.Name = congress.Name;
                context.Congresses.Update(existingCongress);
            }
        }
    }

    private async static Task SaveLegislationDetails(CongressDbContext context, string? billNumber, int congressNumber, string? billType)
    {
        Console.WriteLine($"Saving legislation detail for BillNumber: {billNumber}");
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
        Console.WriteLine("Saving all legislations list.");

        var latestCongresses = await context.Congresses.OrderByDescending(c => c.EndYear).Select(c => c.Number).Take(3).ToListAsync();

        foreach (var congress in latestCongresses)
        {
            string uri = $"https://api.congress.gov/v3/bill/{congress}?api_key={apiKey}";
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
            }
        }
    }

    /// <summary>
    /// Save member data to the database
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private static async Task SaveMembers(CongressDbContext context)
    {
        Console.WriteLine("Saving all members list.");
        for (var i = 0; i <= 15; i++)
        {
            int offset = i * 250;
            string uri = $"https://api.congress.gov/v3/member?offset={offset}&api_key={apiKey}&limit=250";
            var response = await client.GetStringAsync(uri);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var membersRoot = JsonSerializer.Deserialize<JMembersApiRoot>(response, options);

            foreach (var member in membersRoot.Members)
            {
                var existingMember = await context.Members.FirstOrDefaultAsync(m => m.BioguideId == member.BioguideId);

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
                    existingMember.District = member.District;
                    existingMember.PartyName = member.PartyName;
                    existingMember.State = member.State;
                    existingMember.UpdateDate = member.UpdateDate;
                    existingMember.Url = member.Url;
                    context.Members.Update(existingMember);
                }
            }
        }
    }


    private static async Task SaveMemberDetails(CongressDbContext context, string bioGuideId)
    {
        Console.WriteLine($"Saving/Updating all member details for bioGuideId: {bioGuideId}");
        string uri = $"https://api.congress.gov/v3/member/{bioGuideId}?api_key={apiKey}";

        var response = await client.GetStringAsync(uri);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var root = JsonSerializer.Deserialize<JMemberDetailsApiRoot>(response, options);

        var existingMember = await context.Members.Include(m => m.Depiction).FirstOrDefaultAsync(m => m.BioguideId == bioGuideId);

        if (existingMember != null)
        {
            //create a List of Term using the JTerm data from member.Terms
            if (root.Member.Terms != null)
            {
                existingMember.Terms = context.Terms.Where(t => t.MemberId == existingMember.Id).ToList();
                existingMember.District = root.Member.District;

                foreach (var jterm in root.Member.Terms)
                {
                    var existingCongress = context.Congresses.Where(t => t.Number == jterm.Congress)?.FirstOrDefault() ?? null;

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
                            CongressId = existingCongress?.Id ?? null
                        ,
                            MemberId = existingMember.Id
                        });
                    else
                    {
                        var existingTerm = existingMember.Terms.FirstOrDefault(x => x.StartYear == jterm.StartYear && x.EndYear == jterm.EndYear);
                        existingTerm.CongressId = existingCongress?.Id ?? null;
                        context.Terms.Update(existingTerm);
                    }
                }
            }

            if (root.Member.AddressInformation != null && existingMember.AddressInformationId == null)
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

            if (existingMember.Depiction == null)
            {
                if (root.Member.Depiction != null)
                {
                    existingMember.Depiction = new Depiction()
                    {
                        ImageUrl = root.Member.Depiction.ImageUrl
                        ,
                        Attribution = root.Member.Depiction.Attribution
                        ,
                        MemberId = existingMember.Id
                    };
                }
                else
                {
                    existingMember.Depiction = new Depiction()
                    {
                        ImageUrl = ""
                   ,
                        Attribution = ""
                   ,
                        MemberId = existingMember.Id
                    };
                }
            }

            context.Members.Update(existingMember);

            //await context.SaveChangesAsync();
        }
    }

}



