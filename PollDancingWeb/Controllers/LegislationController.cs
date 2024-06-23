using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PollDancingLibrary.Data;
using PollDancingLibrary.DTOs;
using System.Net.Http;
using System.Text.Json;

namespace PollDancingWeb.Controllers
{
    public class LegislationController : Controller
    {
        private readonly ILogger<LegislationController> _logger;
        private readonly CongressDbContext _congressDbContext;
        private readonly IHttpClientFactory _httpClientFactory;

        public LegislationController(ILogger<LegislationController> logger, CongressDbContext dbContext, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _congressDbContext = dbContext;
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult IndexAsync()
        {
            return View();
        }


        public async Task<ActionResult> GetLegislationsAsync(int draw = 1, int length = 10, int start = 1)
        {
            try
            {
                _logger.LogInformation("Get all legislations.");

                start = start / length + 1;

                HttpClient client = _httpClientFactory.CreateClient();
                string apiUrl = $"http://localhost:5184/api/legislation/getall?page={start}"; // Adjust the URL as needed

                var search = Request.Form["search[value]"].FirstOrDefault();
                if (search != null)
                {
                    apiUrl += $"&search={search.ToLower().Trim()}";
                }

                var response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<List<LegislationDTO>>(content);

                    var data = result.Select(bill => new
                    {
                        
                        Id = bill.Id,
                        Number = bill.Number ?? "",
                        Title = bill.Title?.Length > 75 ? bill.Title.Substring(0,75) + "..." : (bill.Title ?? ""),
                        Type = bill.Type ?? "",
                        //Summary = bill.Summaries
                    }).Where(b => !String.IsNullOrEmpty(b.Title)).ToList();

                    HttpClient httpClient = _httpClientFactory.CreateClient();
                    string apiCountUrl = $"http://localhost:5184/api/legislation/getcount"; // Adjust the URL as needed
                    int totalRecords = 0;
                    

                    var responseCount = await client.GetAsync(apiCountUrl);
                    if (responseCount.IsSuccessStatusCode)
                    {
                        totalRecords = int.Parse(await responseCount.Content.ReadAsStringAsync());
                    }

                    int filterRecords = totalRecords;

                    if (!String.IsNullOrEmpty(search))
                    {
                        HttpClient httpClient2 = _httpClientFactory.CreateClient();
                        string apiCountUrl2 = $"http://localhost:5184/api/legislation/getfilteredcount/{search}"; // Adjust the URL as needed
                        var responseCount2 = await client.GetAsync(apiCountUrl2);
                        if (responseCount2.IsSuccessStatusCode)
                        {
                            filterRecords = int.Parse(await responseCount2.Content.ReadAsStringAsync());
                        }
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


        public async Task<ActionResult> GetSponsoredLegislationsAsync(int memberId=0, int draw = 1, int length = 10, int start = 1)
        {
            try
            {
                _logger.LogInformation("Get all legislations.");
                HttpClient client = _httpClientFactory.CreateClient();
                string apiUrl = $"http://localhost:5184/api/legislation/getsponsoredlegislations/{memberId}"; // Adjust the URL as needed

                var search = Request.Form["search[value]"].FirstOrDefault().ToLower().Trim();

                var response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<List<LegislationDTO>>(content);

                    var data = result.Select(bill => new
                    {

                        Id = bill.Id,
                        Number = bill.Number,
                        Title = bill.Title,
                        Type = bill.Type,
                        Summary = bill.Summaries,
                        
                    }).Where(b => !String.IsNullOrEmpty(b.Title)).ToList();

                    //int skip = (start - 1) * length;
                    int totalRecords = data.Count();
                    int filterRecords = totalRecords;
                    data = data.Skip(start).Take(length).ToList();

                    if (!String.IsNullOrEmpty(search))
                    {
                        data = data.Where(bill => bill.Title?.ToLower().Contains(search) ?? false).ToList();
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


        public async Task<ActionResult> GetCoSponsoredLegislationsAsync(int memberId = 0, int draw = 1, int length = 10, int start = 1)
        {
            try
            {
                _logger.LogInformation("Get all legislations.");

                start = start / length + 1;

                HttpClient client = _httpClientFactory.CreateClient();
                string apiUrl = $"http://localhost:5184/api/legislation/getcosponsoredlegislations/{memberId}?page={start}"; // Adjust the URL as needed

                var search = Request.Form["search[value]"].FirstOrDefault().ToLower().Trim();

                var response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<List<LegislationDTO>>(content);

                    var data = result.Select(bill => new
                    {

                        Id = bill.Id,
                        Number = bill.Number,
                        Title = bill.Title,
                        Type = bill.Type,
                        Summary = bill.Summaries
                    }).Where(b => !String.IsNullOrEmpty(b.Title))
                    .ToList();

                    //int skip = (start - 1) * length;
                    int totalRecords = data.Count();
                    int filterRecords = totalRecords;
                    data = data.Skip(start).Take(length).ToList();

                    if (!String.IsNullOrEmpty(search))
                    {
                        data = data.Where(bill => bill.Title?.ToLower().Contains(search) ?? false).ToList();
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


        public async Task<ActionResult> DetailsAsync(int id)
        {
            try
            {
                _logger.LogInformation("Get legislation details.");

                HttpClient client = _httpClientFactory.CreateClient();
                string apiUrl = $"http://localhost:5184/api/legislation/{id}"; // Adjust the URL as needed

                var response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<RepresentativeDTO>(content);

                    return View("LegislationDetails", result);
                }

                return Json(new { error = "Failed to fetch data from API" });
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
