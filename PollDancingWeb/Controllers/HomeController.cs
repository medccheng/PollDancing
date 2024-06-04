using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PollDancingLibrary.Data;
using PollDancingLibrary.Models;
using PollDancingWeb.Models;
using System.Diagnostics;
using NLog;

namespace PollDancingWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CongressDbContext _congressDbContext;
        public HomeController(ILogger<HomeController> logger, CongressDbContext dbContext)
        {
            _logger = logger;
            _congressDbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

    

        //write api to get all members
        public async Task<IActionResult> GetMembersAsync(int draw = 1, int length = 10, int start = 1, dynamic? search=null)
        {
            try
            {
                _logger.LogInformation("Get members list.");
                
                // Calculate the number of records to skip
                int skip = draw * length;
                //int latestCongressNumber = await _congressDbContext.Congresses.MaxAsync(c => c.Number);
                var activeMembers = await _congressDbContext.Members
                                    .Where(m => m.Terms.Any(t => t.EndYear == null || t.EndYear >= DateTime.Now.Year))
                                    .OrderBy(m => m.Id) // Assuming you are ordering by Id or adjust as needed
                                    .Skip(skip)
                                    .Take(length)
                                    .ToListAsync();


                // Get total number of records
                //var allActiveMembers = await _congressDbContext.Members.Include(m=>m.Terms).Where(m => m.Terms.Any(t => t.EndYear == null || t.EndYear >= DateTime.Now.Year)).ToListAsync();
                //int recordsTotal = allActiveMembers.Count;
                int? recordsTotal = 100;
                             

                var data = new List<object>();
                foreach (var member in activeMembers)
                {
                    //var sponsoredLegislations = await _congressDbContext.SponsoredLegislations.Where(s => s.MemberId == member.Id).ToListAsync();

                    data.Add(new
                    {
                        Id =member.Id,
                        BioguideId = member.BioguideId ?? "",
                        Name = member.Name ?? "",
                        State = member.State ?? "",
                        District =member.District ?? 0,
                        PartyName= member.PartyName ?? "",
                        //Office = member.AddressInformation?.OfficeAddress ?? "",
                        UpdateDate = member.UpdateDate?.ToString("yyyy-MM-dd") ?? "",
                        Type = member.Terms.OrderByDescending(t => t.EndYear)?.FirstOrDefault()?.MemberType ?? "",
                        Image = member.Depiction?.ImageUrl ?? "",
                        //SponsoredLegislations = sponsoredLegislations?.Count ?? 0,
                    });
                }

                var recordsFiltered = recordsTotal;

                
                // Return the data and pagination info
                return Json(new { data, draw, recordsTotal, recordsFiltered });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return Json(new { error = ex.Message });
            }
        }

        public IActionResult GetDetails(int id)
        {
            var member = _congressDbContext.Members
                .Include(m => m.Terms)
                .Include(m => m.AddressInformation)
                .Include(m => m.Depiction)
                .Include(m => m.Terms)
                .Include(m => m.SponsoredLegislations)
                .Include(m => m.CosponsoredLegislations)
                .Include(m => m.MemberLegislationVotes)
                .FirstOrDefault(m => m.Id == id);

            var terms = member.Terms.OrderByDescending(t => t.EndYear).FirstOrDefault();
            List<TermViewModel> termsViewModel = new List<TermViewModel>();
            

            foreach (var term in member.Terms)
            {
               termsViewModel.Add(new TermViewModel()
               {
                   StartYear = (int)term.StartYear,
                   EndYear = term.EndYear,
                   MemberType = term.MemberType,
                   CongressId = (int)term.CongressId,                   
                   StateName = term.StateName,
                   StateCode = term.StateCode,
                   Congress = new CongressViewModel()
                   {
                       Id = term.Congress.Id,
                       StartYear = term.Congress.StartYear,
                       EndYear = term.Congress.EndYear,
                       Number = term.Congress.Number,
                       Name = term.Congress.Name,
                   }
               });
            }

            List<MemberLegislationVotesViewModel> memberLegislationVotes = new List<MemberLegislationVotesViewModel>();
            foreach (var x in member.MemberLegislationVotes)
            {
                memberLegislationVotes.Add(new MemberLegislationVotesViewModel()
                {
                    MemberId = x.MemberId,
                    LegislationId = x.LegislationId,
                    Vote = x.Vote,
                    LegislationTitle = x.Legislation.Title
                });
            }

            List<SponsoredLegislationViewModel> sponsoredLegislations = new List<SponsoredLegislationViewModel>();
            foreach (var x in member.SponsoredLegislations)
            {
                sponsoredLegislations.Add(new SponsoredLegislationViewModel()
                {
                    MemberId = x.MemberId,
                    LegislationId = x.LegislationId,                    
                    LegislationTitle = x.Legislation.Title,
                    LegislationDescription = x.Legislation.Summaries.FirstOrDefault().ToString()
                });
            }

            List<SponsoredLegislationViewModel> coSponsoredLegislations = new List<SponsoredLegislationViewModel>();
            foreach (var x in member.CosponsoredLegislations)
            {
                coSponsoredLegislations.Add(new SponsoredLegislationViewModel()
                {
                    MemberId = x.MemberId,
                    LegislationId = x.LegislationId,
                    LegislationTitle = x.Legislation.Title,
                    LegislationDescription = x.Legislation.Summaries.FirstOrDefault().ToString()
                });
            }

            MemberViewModel result = new MemberViewModel()
            {
                BioguideId = member.BioguideId ?? "",
                Name = member.Name ?? "",
                State = member.State ?? "",
                District = member.District.ToString(),
                PartyName = member.PartyName ?? "",
                UpdateDate = (DateTime)member.UpdateDate,
                AddressInformation = new AddressInformationViewModel()
                { 
                    MemberId = member.AddressInformation.MemberId,
                    OfficeAddress = member.AddressInformation.OfficeAddress,
                    City = member.AddressInformation.City,
                    PhoneNumber = member.AddressInformation.PhoneNumber,
                    District = member.AddressInformation.District,
                },
                Depiction = new DepictionViewModel()
                {
                    MemberId = member.Depiction.MemberId,
                    ImageUrl = member.Depiction.ImageUrl,
                    Attribution = member.Depiction.Attribution,
                },
                Terms = termsViewModel.OrderByDescending(x => x.StartYear).ToList(),
                SponsoredLegislations = sponsoredLegislations,
                MemberLegislationVotes = memberLegislationVotes,
                CosponsoredLegislations = coSponsoredLegislations
            };
            

            return View("MemberDetails", result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
