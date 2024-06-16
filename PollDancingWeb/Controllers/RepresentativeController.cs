using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PollDancingLibrary.Data;
using PollDancingLibrary.DTOs;
using System.Net.Http;
using System.Text.Json;

namespace PollDancingWeb.Controllers
{
    public class RepresentativeController : Controller
    {
        private readonly ILogger<RepresentativeController> _logger;
        private readonly CongressDbContext _congressDbContext;
        private readonly IHttpClientFactory _httpClientFactory;

        public RepresentativeController(ILogger<RepresentativeController> logger, CongressDbContext dbContext, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _congressDbContext = dbContext;
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult IndexAsync()
        {
            return View();
        }


        public async Task<ActionResult> GetMembersAsync(int draw = 1, int length = 10, int start = 1, dynamic? search = null)
        {
            try
            {
                _logger.LogInformation("Get senators list.");

                start = start / length + 1;

                HttpClient client = _httpClientFactory.CreateClient();
                string apiUrl = $"http://localhost:5184/api/representative/getall?page={start}"; // Adjust the URL as needed

                var response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<List<RepresentativeDTO>>(content);

                    var data = result.Select(member => new
                    {
                        Id = member.Id,
                        BioguideId = member.BioguideId,
                        Name = member.Name,
                        State = member.State,
                        PartyName = member.PartyName,
                        UpdateDate = member.UpdateDate,
                        District = member.District,
                        Image = member.Image
                    }).ToList();

                    HttpClient httpClient = _httpClientFactory.CreateClient();
                    string apiCountUrl = $"http://localhost:5184/api/representative/getcount"; // Adjust the URL as needed
                    int totalRecords = 0;

                    var responseCount = await client.GetAsync(apiCountUrl);
                    if (responseCount.IsSuccessStatusCode)
                    {
                        totalRecords = int.Parse(await responseCount.Content.ReadAsStringAsync());
                    }

                    // Assuming your API returns total count of records, adjust accordingly
                    return Json(new { data, draw, recordsTotal = totalRecords, recordsFiltered = totalRecords });
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

        // GET: RepresentativeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
