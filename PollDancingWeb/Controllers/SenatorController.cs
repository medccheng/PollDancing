using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PollDancingLibrary.Data;

namespace PollDancingWeb.Controllers
{
    public class SenatorController : Controller
    {
        private readonly ILogger<SenatorController> _logger;
        private readonly CongressDbContext _congressDbContext;
        public SenatorController(ILogger<SenatorController> logger, CongressDbContext dbContext)
        {
            _logger = logger;
            _congressDbContext = dbContext;
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

                // Calculate the number of records to skip
                int skip = draw * length;
                var activeMembers = await _congressDbContext.Members
                                    .Include(m => m.Depiction)
                                    .Include(m => m.Terms)
                                    .Where(m => m.Terms.Any(t => (((t.EndYear == null || t.EndYear >= DateTime.Now.Year) && t.MemberType == "Senator"))))
                                    .OrderBy(m => m.Id)
                                    .Skip(skip)
                                    .Take(length)
                                    .ToListAsync();

                int? recordsTotal = 100;


                var data = new List<object>();
                foreach (var member in activeMembers)
                {
                    data.Add(new
                    {
                        Id = member?.Id ?? 0,
                        BioguideId = member?.BioguideId ?? "",
                        Name = member?.Name ?? "",
                        State = member?.State ?? "",
                        District = member?.District ?? 0,
                        PartyName = member?.PartyName ?? "",
                        UpdateDate = member?.UpdateDate?.ToString("yyyy-MM-dd") ?? "",
                        Type = member.Terms.OrderByDescending(t => t.EndYear)?.FirstOrDefault()?.MemberType ?? "",
                        Image = member?.Depiction?.ImageUrl ?? "",
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

        // GET: SenatorController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SenatorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SenatorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SenatorController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SenatorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SenatorController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SenatorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
