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
                _logger.LogDebug("Get members list.");
                
                // Calculate the number of records to skip
                int skip = start;
                //int latestCongressNumber = await _congressDbContext.Congresses.MaxAsync(c => c.Number);
                var activeMembers = await _congressDbContext.Members
                                    .Include(m => m.Terms)
                                    .Where(m => m.Terms.Any(t => t.EndYear == null || t.EndYear >= DateTime.Now.Year))
                                    .OrderBy(m => m.Id) // Assuming you are ordering by Id or adjust as needed
                                    .Skip(skip)
                                    .Take(length)
                                    .ToListAsync();


                // Get total number of records
                var allActiveMembers = await _congressDbContext.Members.Include(m=>m.Terms).Where(m => m.Terms.Any(t => t.EndYear == null || t.EndYear >= DateTime.Now.Year)).ToListAsync();
                int recordsTotal = allActiveMembers.Count;
                             

                var data = new List<object>();
                foreach (var member in activeMembers)
                {
                    var sponsoredLegislations = await _congressDbContext.SponsoredLegislations.Where(s => s.MemberId == member.Id).ToListAsync();

                    data.Add(new
                    {
                        Id =member.Id,
                        BioguideId = member.BioguideId ?? "",
                        Name = member.Name ?? "",
                        State = member.State ?? "",
                        District =member.District ?? 0,
                        PartyName= member.PartyName ?? "",
                        Office = member.AddressInformation?.OfficeAddress ?? "",
                        UpdateDate = member.UpdateDate?.ToString("yyyy-MM-dd") ?? "",
                        Type = member.Terms.OrderByDescending(t => t.EndYear)?.FirstOrDefault().MemberType ?? "",
                        Image = member.Depiction?.ImageUrl ?? "",
                        SponsoredLegislations = sponsoredLegislations?.Count ?? 0,
                    });
                }

                var recordsFiltered = recordsTotal;

                // Return the data and pagination info
                return Json(new { data, draw, recordsTotal, recordsFiltered });
            }
            catch (Exception ex)
            {
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
                Terms = termsViewModel,
                //SponsoredLegislations = member.SponsoredLegislations
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
