using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PollDancingLibrary.Data;
using PollDancingLibrary.DTOs;
using PollDancingLibrary.Models;
using System.Net.Http;
using System.Text.Json;

namespace PollDancingWeb.Controllers
{
    public class ScoreCardController : Controller
    {
        private readonly ILogger<ScoreCardController> _logger;
        private readonly CongressDbContext _congressDbContext;
        private readonly IHttpClientFactory _httpClientFactory;

        public ScoreCardController(ILogger<ScoreCardController> logger, CongressDbContext dbContext, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _congressDbContext = dbContext;
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult IndexAsync()
        {
            return View();
        }


        public async Task<ActionResult> GetScoreCards(int draw = 1, int length = 10, int start = 1, int memberId=0)
        {
            try
            {
                _logger.LogInformation($"Get all score cards of {memberId}.");

                start = start / length + 1;

                HttpClient client = _httpClientFactory.CreateClient();
                string apiUrl = $"http://localhost:5184/api/scorecard/getscorecards/{memberId}"; // Adjust the URL as needed
                var search = Request.Form["search[value]"].FirstOrDefault().ToLower().Trim();

                var response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<List<ScoreCardDTO>>(content);

                    var data = result.Select(sc => new
                    {
                        Subject = sc.Subject,
                        Score = sc.Score,
                        Comment = sc.Comment
                    }).ToList();

                    int totalRecords = data.Count();
                    int skip = (start - 1) * length;
                    data = data.Skip(skip).Take(length).ToList();  
                    
                    int filterRecords = totalRecords;
                    if (!string.IsNullOrEmpty(search))
                    {
                        data = data.Where(x => x.Subject.ToLower().Contains(search)).ToList();
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
