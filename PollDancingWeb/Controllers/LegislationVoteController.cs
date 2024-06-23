using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PollDancingLibrary.Data;
using PollDancingLibrary.DTOs;
using System.Net.Http;
using System.Text.Json;

namespace PollDancingWeb.Controllers
{
    public class LegislationVoteController : Controller
    {
        private readonly ILogger<LegislationVoteController> _logger;
        private readonly CongressDbContext _congressDbContext;
        private readonly IHttpClientFactory _httpClientFactory;

        public LegislationVoteController(ILogger<LegislationVoteController> logger, CongressDbContext dbContext, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _congressDbContext = dbContext;
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult IndexAsync()
        {
            return View();
        }


        public async Task<ActionResult> GetBillVotes(int draw = 1, int length = 10, int start = 1, int memberId=0)
        {
            try
            {
                _logger.LogInformation($"Get all bill votes of {memberId}.");

                start = start / length + 1;

                HttpClient client = _httpClientFactory.CreateClient();
                string apiUrl = $"http://localhost:5184/api/legislationvote/getbillvotes/{memberId}"; // Adjust the URL as needed
                var search = Request.Form["search[value]"].FirstOrDefault().ToLower().Trim();

                var response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<List<LegislationVoteDTO>>(content);

                    var data = result.Select(bill => new
                    {
                        
                        Id = bill.Id,
                        Title = bill.Title?.Length > 75 ? bill.Title.Substring(0,75) + "..." : (bill.Title ?? ""),
                        Yes = bill.Vote.ToLower().Contains("yes") || bill.Vote.ToLower().Contains("aye") || bill.Vote.ToLower().Contains("yea") ? "yes" : "",
                        No = (bill.Vote.ToLower().Contains("no") && !bill.Vote.ToLower().Contains("not voting")) || bill.Vote.ToLower().Contains("nay") ? "no" : "",
                        NotVoting = bill.Vote.ToLower().Contains("not voting") ? "not voting" : "",
                    }).ToList();

                    int totalRecords = data.Count();
                    int skip = (start - 1) * length;
                    data = data.Skip(skip).Take(length).ToList();  
                    
                    int filterRecords = totalRecords;
                    if (!string.IsNullOrEmpty(search))
                    {
                        data = data.Where(x => x.Title.ToLower().Contains(search)).ToList();
                        filterRecords = data.Count();
                    }

                    // Assuming your API returns total count of records, adjust accordingly
                    return Json(new { data, draw, recordsTotal = totalRecords, recordsFiltered = filterRecords });
                }
                else
                {
                    _logger.LogError($"Failed to fetch data: {response.StatusCode}");
                    return Json(new { error = "Failed to fetch data from API" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return Json(new { error = ex.Message });
            }
        }


        // GET: RepresentativeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RepresentativeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }

        // GET: RepresentativeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RepresentativeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }

        // GET: RepresentativeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RepresentativeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }
    }
}
