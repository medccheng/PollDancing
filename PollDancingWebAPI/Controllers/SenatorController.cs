using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PollDancingLibrary.Data;

namespace PollDancingWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SenatorController : ControllerBase
    {
        private readonly ILogger<SenatorController> _logger;
        private readonly CongressDbContext _congressDbContext;
        public SenatorController(ILogger<SenatorController> logger, CongressDbContext dbContext)
        {
            _logger = logger;
            _congressDbContext = dbContext;
        }

        [HttpGet(Name = "GetSenators")]
        public async Task<ActionResult> Get()
        {
            try
            {
                _logger.LogInformation("Get senators list.");
                var activeMembers = await _congressDbContext.Members
                                    .Where(m => m.Terms.Any(t => ((t.EndYear == null || t.EndYear >= DateTime.Now.Year) && t.MemberType == "Senator")))
                                    .OrderBy(m => m.Id)
                                    .Skip(0)
                                    .Take(2)
                                    .ToListAsync();

                int? recordsTotal = 100;


                var data = new List<object>();
                foreach (var member in activeMembers)
                {
                    data.Add(new
                    {
                        Id = member.Id,
                        BioguideId = member.BioguideId ?? "",
                        Name = member.Name ?? "",
                        State = member.State ?? "",
                        District = member.District ?? 0,
                        PartyName = member.PartyName ?? "",
                        UpdateDate = member.UpdateDate?.ToString("yyyy-MM-dd") ?? "",
                        Image = member.Depiction?.ImageUrl ?? "",
                    });
                }

                var recordsFiltered = recordsTotal;

                // Return the data and pagination info
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
