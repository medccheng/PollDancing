using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PollDancingLibrary.Data;
using PollDancingLibrary.Models;
using PollDancingWeb.Models;
using System.Diagnostics;


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
            // Calculate the number of records to skip
            int skip = start;
            
            //get the value property from the search object
            //var searchValue = search?.value;


            // Get total number of records
            int recordsTotal = await _congressDbContext.Members.CountAsync();

            if (skip > recordsTotal)
            {
                   skip = 0;
            }

            // Fetch paginated data
            var members = await _congressDbContext.Members
                                .Include(x => x.AddressInformation)
                                .Include(x => x.Depiction)
                                .OrderBy(m => m.Id) // Assuming you are ordering by Id or adjust as needed
                                .Skip(skip)
                                .Take(length)
                                .ToListAsync();

            var data = new List<object>();
            foreach (var member in members)
            {
                var terms = await _congressDbContext.Terms
                                .Where(t => t.MemberId == member.Id)
                                .OrderByDescending(t => t.EndYear)
                                .ToListAsync();
                var sponsoredLegislations = await _congressDbContext.SponsoredLegislations.Where(s => s.MemberId == member.Id).ToListAsync();

                data.Add(new
                {
                    member.Id,
                    member.BioguideId,
                    member.Name,
                    member.State,
                    member.District,
                    member.PartyName,
                    //member.Url,
                    Office = member.AddressInformation?.OfficeAddress ?? "",
                    UpdateDate = member.UpdateDate?.ToString("yyyy-MM-dd"),
                    Type = terms.FirstOrDefault()?.MemberType ?? "",
                    Image = member.Depiction?.ImageUrl ?? "",
                    SponsoredLegislations = sponsoredLegislations?.Count ?? 0,
                });
            }

            var recordsFiltered = recordsTotal;

            // Return the data and pagination info
            return Json(new { data, draw, recordsTotal, recordsFiltered });
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
