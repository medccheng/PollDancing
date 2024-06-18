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
using System;

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
            optionsBuilder.UseSqlServer("Server=DESKTOP-LSNPVK6\\SQLEXPRESS;Database=PollDancingDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");

            using (var context = new CongressDbContext(optionsBuilder.Options))
            {
                bool continueRunning = true;

                while (continueRunning)
                {
                    Console.WriteLine("Choose an option:");
                    Console.WriteLine("1 - Save Congresses");
                    Console.WriteLine("2 - Save Members");
                    Console.WriteLine("3 - Save Member Details for Active Members");
                    Console.WriteLine("4 - Save All Legislations Data");
                    Console.WriteLine("5 - Save Specific Legislation Details");
                    Console.WriteLine("6 - Save All Data");
                    Console.WriteLine("7 - Save Current");
                    Console.WriteLine("8 - Save Sponsored Legislations");
                    Console.WriteLine("9 - Save CoSponsored Legislations");
                    Console.WriteLine("10 - Do Nothing (Exit)");
                    Console.Write("Enter option: ");

                    var option = Console.ReadLine();
                    var currentCongress = context.Congresses.FirstOrDefault(c => c.IsCurrent);
                    var currentLegislations = context.Legislations.Where(x => x.CongressId.Equals(currentCongress.Number)).OrderByDescending(x => x.IntroducedDate).ToList().Take(500);
                    //var currentLegislations = context.Legislations.Where(x => x.CongressId.Equals(currentCongress.Number)).Skip(2000).Take(500).OrderByDescending(x => x.IntroducedDate).ToList();
                    switch (option)
                    {
                        case "1":
                            await SaveCongresses(context);
                            await context.SaveChangesAsync();
                            break;
                        case "2":
                            await SaveMembers(context);
                            await context.SaveChangesAsync();
                            break;
                        case "3":
                            foreach (var member in context.Members)
                            {
                                await SaveMemberDetails(context, member.BioguideId);
                            }
                            await context.SaveChangesAsync();
                            break;
                        case "4":
                            Console.WriteLine("Saving all legislations data.");
                            await SaveLegislations(context);
                            await context.SaveChangesAsync();
                            break;
                        case "5":
                            foreach (var legislation in currentLegislations)
                            {
                                await SaveLegislationDetails(context, legislation.Number, (int)legislation.CongressId, legislation.Type);                                
                            }
                            
                            break;
                        case "6":
                            await SaveCongresses(context);
                            await SaveMembers(context);
                            await SaveLegislations(context);
                            await context.SaveChangesAsync();
                            foreach (var member in context.Members.Include(m => m.Terms).Where(m => !m.Terms.Any() && m.UpdateDate > DateTime.Parse("1/1/2023")).Take(200))
                            {
                                await SaveMemberDetails(context, member.BioguideId);
                            }

                            foreach (var legislation in currentLegislations)
                            {
                                await SaveLegislationDetails(context, legislation.Number, (int)legislation.CongressId, legislation.Type);
                            }
                            await context.SaveChangesAsync();
                            await SaveCurrentCongress(context);
                            await SaveSponsoredLegislations(context);
                            await SaveCosponsoredLegislations(context);
                            await context.SaveChangesAsync();
                            break;
                        case "7":
                            Console.WriteLine("Get current congress...");
                            await SaveCurrentCongress(context);
                            await context.SaveChangesAsync();
                            break;
                        case "8":
                            Console.WriteLine("Get current members sponsored legislations...");
                            await SaveSponsoredLegislations(context);
                            await context.SaveChangesAsync();
                            break;
                        case "9":
                            Console.WriteLine("Get current members cosponsored legislations...");
                            await SaveCosponsoredLegislations(context);
                            await context.SaveChangesAsync();
                            break;
                        case "10":
                            Console.WriteLine("No action taken. Exiting...");
                            continueRunning = false;  // Set flag to false to exit loop
                            break;
                        default:
                            Console.WriteLine("Invalid option, please choose a correct number.");
                            break;
                    }                    
                }
            }

            Console.WriteLine("Operation complete.");
            Console.ReadLine();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
        }
    }

    private static async Task SaveSponsoredLegislations(CongressDbContext context)
    {
        var currentMembers = context.Members.Include(m => m.Terms).Where(m => m.Terms.Any(t => t.Congress.IsCurrent && t.EndYear == null)).ToList();
        int count = currentMembers.Count;
        int counter = 1;
        foreach (var member in currentMembers)
        {
            Console.WriteLine($"{counter} of {count}");
            Console.WriteLine($"Saving sponsored legislations for {member.Name}...");
            string uri = $"https://api.congress.gov/v3/member/{member.BioguideId}/sponsored-legislation?api_key={apiKey}";
            var response = await client.GetStringAsync(uri);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var sponsoredLegislationsRoot = JsonSerializer.Deserialize<JSponsoredLegislationsApiRoot>(response, options);

            foreach (var sponsoredLegislation in sponsoredLegislationsRoot.SponsoredLegislations)
            {
                var existingLegislation = await context.Legislations.FirstOrDefaultAsync(m => m.Number == sponsoredLegislation.Number && m.CongressId == sponsoredLegislation.Congress);

                if (existingLegislation == null)
                {
                    existingLegislation = new Legislation()
                    {
                        Number = sponsoredLegislation.Number,
                        CongressId = sponsoredLegislation.Congress,
                        Title = sponsoredLegislation.Title,
                        Type = sponsoredLegislation.Type,
                        UpdateDate = sponsoredLegislation.LatestAction.ActionDate,
                        Url = sponsoredLegislation.Url,
                        IntroducedDate = sponsoredLegislation.IntroducedDate
                    };
                    context.Legislations.Add(existingLegislation);
                    await context.SaveChangesAsync();
                }

                var existingSponsoredLegislation = await context.SponsoredLegislations.FirstOrDefaultAsync(m => m.Legislation.Number == sponsoredLegislation.Number &&  m.Member.BioguideId == member.BioguideId);

                if (existingSponsoredLegislation == null)
                {
                    var existingLegislation2 = await context.Legislations.FirstOrDefaultAsync(m => m.Number == sponsoredLegislation.Number && m.CongressId == sponsoredLegislation.Congress);

                    if (existingLegislation2 != null)
                    {
                        context.SponsoredLegislations.Add(new SponsoredLegislation()
                        {
                            LegislationId = existingLegislation2.Id
                            ,
                            MemberId = member.Id
                        });
                        await context.SaveChangesAsync();
                    }
                }
            }

            counter++;
        }
    }

    private static async Task SaveCosponsoredLegislations(CongressDbContext context)
    {
        var currentMembers = context.Members.Include(m => m.Terms).Where(m => m.Terms.Any(t => t.Congress.IsCurrent && t.EndYear == null)).ToList();
        int count = currentMembers.Count;
        int counter = 1;

        foreach (var member in currentMembers)
        {
            Console.WriteLine($"{counter} of {count}");
            Console.WriteLine($"Saving cosponsored legislations for {member.Name}...");
            string uri = $"https://api.congress.gov/v3/member/{member.BioguideId}/cosponsored-legislation?api_key={apiKey}";
            var response = await client.GetStringAsync(uri);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var cosponsoredLegislationsRoot = JsonSerializer.Deserialize<JCosponsoredLegislationsApiRoot>(response, options);

            foreach (var cosponsoredLegislation in cosponsoredLegislationsRoot.CosponsoredLegislations)
            {
                var existingLegislation = await context.Legislations.FirstOrDefaultAsync(m => m.Number == cosponsoredLegislation.Number && m.CongressId == cosponsoredLegislation.Congress);

                if (existingLegislation == null)
                {
                    existingLegislation = new Legislation()
                    {
                        Number = cosponsoredLegislation.Number,
                        CongressId = cosponsoredLegislation.Congress,
                        Title = cosponsoredLegislation.Title,
                        Type = cosponsoredLegislation.Type,
                        UpdateDate = cosponsoredLegislation.LatestAction.ActionDate,
                        Url = cosponsoredLegislation.Url,
                        IntroducedDate = cosponsoredLegislation.IntroducedDate
                    };
                    context.Legislations.Add(existingLegislation);
                    await context.SaveChangesAsync();
                }

                var existingCosponsoredLegislation = await context.CosponsoredLegislations.FirstOrDefaultAsync(m => m.Legislation.Number == cosponsoredLegislation.Number && m.Member.BioguideId == member.BioguideId);

                if (existingCosponsoredLegislation == null)
                {
                    var existingLegislation2 = await context.Legislations.FirstOrDefaultAsync(m => m.Number == cosponsoredLegislation.Number && m.CongressId == cosponsoredLegislation.Congress);

                    if (existingLegislation2 != null)
                    {
                        context.CosponsoredLegislations.Add(new CosponsoredLegislation()
                        {
                            LegislationId = existingLegislation2.Id
                            ,
                            MemberId = member.Id
                        });
                        await context.SaveChangesAsync();
                    }
                }
            }
            counter++;
        }
    }

    private static async Task SaveCurrentCongress(CongressDbContext context)
    {
        Console.WriteLine("Saving all current congress entities.");
        string uri = $"https://api.congress.gov/v3/congress/current?api_key={apiKey}";
        var response = await client.GetStringAsync(uri);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var congressesRoot = JsonSerializer.Deserialize<JCongressesApiRoot>(response, options);

        if (congressesRoot.Congress != null)
        {
            var congress = congressesRoot.Congress;

            string pattern = @"\d+";
            Match match = Regex.Match(congress.Name, pattern);
            int id = 0;

            var currentCongress = await context.Congresses.FirstOrDefaultAsync(c => c.IsCurrent);

            if (currentCongress != null)
            {
                currentCongress.IsCurrent = false;
                context.Congresses.Update(currentCongress);
            }

            var existingCongress = await context.Congresses.FirstOrDefaultAsync(c => c.Number.Equals(congress.Number));

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
                    ,
                    IsCurrent = true
                });
            }
            else
            {
                existingCongress.Number = (int.TryParse(match.Value, out id) ? id : 0);
                existingCongress.Name = congress.Name;
                existingCongress.IsCurrent = true;
                context.Congresses.Update(existingCongress);
            }
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
            if (bill.Congress != 0 && bill.Type != null && bill.Number != null)
            {
                await SaveBillActions(bill.Congress, bill.Type, bill.Number, context);
                //await SaveBillSummaries(bill.Congress, bill.Type, bill.Number, existingLegislation);
            }

            await context.SaveChangesAsync();
        }
    }

    private static async Task SaveLegislations(CongressDbContext context)
    {
        Console.WriteLine("Saving all legislations list.");

        // Retrieve bills in the last 5 congresses
        //var latestCongresses = await context.Congresses.OrderByDescending(c => c.EndYear).Select(c => c.Number).Take(200).ToListAsync();
        var latestCongresses = await context.Congresses.Where(x =>x.IsCurrent).Select(c => c.Number).ToListAsync();

        foreach (var congress in latestCongresses)
        {
            string uri = $"https://api.congress.gov/v3/bill/{congress}?api_key={apiKey}";
            var response = await client.GetStringAsync(uri);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var legislationsRoot = JsonSerializer.Deserialize<JLegislationApiRoot>(response, options);

            foreach (var bill in legislationsRoot.Bills)
            {
                var existingBill = await context.Legislations.FirstOrDefaultAsync(m => m.Number == bill.Number && m.CongressId == bill.Congress);

                if (existingBill == null)
                {
                    context.Legislations.Add(new Legislation()
                    {
                        Number = bill.Number
                        ,
                        CongressId = bill.Congress
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

    private static async Task SaveBillSummaries(int congress, string? type, string? number, Legislation bill)
    {
        Console.WriteLine("Saving all legislations summaries.");
        string uri = $"https://api.congress.gov/v3/bill/{congress}/{type.ToLower()}/{number}/summaries?api_key={apiKey}";
        var response = await client.GetStringAsync(uri);

        if (bill != null)
        {
            bill.Summaries = response;
        }
    }

    private static async Task SaveBillActions(int congress, string? type, string? number, CongressDbContext context)
    {
        Console.WriteLine("Saving all legislations actions.");
        string uri = $"https://api.congress.gov/v3/bill/{congress}/{type.ToLower()}/{number}/actions?api_key={apiKey}";
        var response = await client.GetStringAsync(uri);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var actionsRoot = JsonSerializer.Deserialize<JActionApiRoot>(response, options);

        var existingBill = await context.Legislations.FirstOrDefaultAsync(m => m.Number == number);

        foreach (var action in actionsRoot.Actions)
        {
            var existingAction = await context.Actions.FirstOrDefaultAsync(m => m.ActionCode.Equals(action.ActionCode) && m.LegislationId.Equals(existingBill.Id));

            if (existingAction == null && !string.IsNullOrEmpty(action.RecordedVotes?.FirstOrDefault()?.Url))
            {
                context.Actions.Add(new PollDancingLibrary.Models.Action()
                {
                    Text = action.Text,
                    ActionCode = action.ActionCode,
                    ActionDate = action.ActionDate,
                    Type = action.Type,
                    LegislationId = existingBill.Id,
                    //RecordedVotesUrl = action.RecordedVotes?.FirstOrDefault()?.Url,
                    RecordedVotes = await GetXMLVotes(action.RecordedVotes?.FirstOrDefault()?.Url)
                });
            }
        }
    }

    private static async Task<string?> GetXMLVotes(string? url)
    {
        if (!string.IsNullOrEmpty(url))
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string xmlContent = await response.Content.ReadAsStringAsync();
                    return xmlContent;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Save member data to the database
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private static async Task SaveMembers(CongressDbContext context)
    {
        Console.WriteLine("Saving all members list.");
        for (var i = 0; i <= 16; i++)
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



