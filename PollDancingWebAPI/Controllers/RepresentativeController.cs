using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PollDancingLibrary.Data;
using System.Linq;

namespace PollDancingWebAPI.Controllers
{
    [ApiController]
    [Route("api/representative")]
    public class RepresentativeController : ControllerBase
    {
        private readonly ILogger<SenatorController> _logger;
        private readonly CongressDbContext _congressDbContext;

        public RepresentativeController(ILogger<SenatorController> logger, CongressDbContext dbContext)
        {
            _logger = logger;
            _congressDbContext = dbContext;
        }

        // Get count of active representatives
        [HttpGet("getcount")]
        public async Task<ActionResult> GetCount()
        {
            int count = 0;
            try
            {
                var query = _congressDbContext.Members
                    .Include(m => m.Depiction)
                    .Include(m => m.Terms)
                    .Where(m => m.Terms.Any(t => t.MemberType == "Representative" &&
                                                  (t.EndYear == null || t.EndYear >= DateTime.Now.Year)));

                count = await query.CountAsync();
               
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching count of representatives: {ex}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // Get a list of senators with pagination
        [HttpGet("getall")]
        public async Task<ActionResult> GetAll(int page = 1)
        {
            const int pageSize = 10;
            try
            {
                var query = _congressDbContext.Members
                    .Include(m => m.Depiction)
                    .Include(m => m.Terms)
                    .Where(m => m.Terms.Any(t => t.MemberType == "Representative" &&
                                                  (t.EndYear == null || t.EndYear >= DateTime.Now.Year)))
                    .OrderBy(m => m.Id);

                var senators = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                var responseData = senators.Select(m => new
                {
                    Id = m.Id,
                    BioguideId = m.BioguideId,
                    Name = m.Name,
                    State = m.State,
                    PartyName = m.PartyName,
                    UpdateDate = m.UpdateDate?.ToString("yyyy-MM-dd"),
                    District = m.District,
                    Type ="",
                    Image = m.Depiction?.ImageUrl
                }).ToList();

                return Ok(responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching representatives: {ex}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // Get specific senator details
        [HttpGet("{memberId}")]
        public async Task<ActionResult> GetDetail(int memberId)
        {
            if (memberId <= 0)
            {
                return BadRequest("Invalid member id.");
            }

            try
            {
                var rep = await _congressDbContext.Members
                    .Include(m => m.Depiction)
                    .Include(m => m.Terms)
                    .FirstOrDefaultAsync(m => m.Id == memberId &&
                                             m.Terms.Any(t => t.MemberType == "Representative"));

                if (rep == null)
                {
                    return NotFound("Representative not found.");
                }

                return Ok(new
                {
                    Id = rep.Id,
                    BioguideId = rep.BioguideId,
                    Name = rep.Name,
                    State = rep.State,
                    PartyName = rep.PartyName,
                    UpdateDate = rep.UpdateDate?.ToString("yyyy-MM-dd"),
                    District = rep.District,
                    Type = rep.Terms.OrderByDescending(t => t.EndYear).FirstOrDefault()?.MemberType,
                    Image = rep.Depiction?.ImageUrl
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching details for representative {memberId}: {ex}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
