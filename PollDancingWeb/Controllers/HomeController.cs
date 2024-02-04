using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PollDancingLibrary.Data;
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
        public async Task<IActionResult> GetMembersAsync()
        {
            var members = await _congressDbContext.Members.ToListAsync();
            var data = new List<object>();
            foreach (var member in members)
            {
                member.Terms = await _congressDbContext.Terms.Where(t => t.MemberId == member.Id).ToListAsync();
                data.Add(new
                {
                    Id = member.Id,
                    BioguideId = member.BioguideId,
                    Name = member.Name,
                    State = member.State,
                    District = member.District,
                    PartyName = member.PartyName,
                    Url = member.Url,
                    UpdateDate = member.UpdateDate?.ToString("yyyy-MM-dd"),
                    Type = member.Terms?.FirstOrDefault()?.Chamber ?? "",
                });
            }
            
            return Json(data);
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
