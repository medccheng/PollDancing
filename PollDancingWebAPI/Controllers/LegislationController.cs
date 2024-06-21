using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PollDancingLibrary.Data;
using PollDancingLibrary.DTOs;
using PollDancingLibrary.Interfaces;
using PollDancingLibrary.Models;
using System.Linq;

namespace PollDancingWebAPI.Controllers
{
    [ApiController]
    [Route("api/legislation")]
    public class LegislationController : ControllerBase, IMember
    {
        private readonly ILogger<LegislationController> _logger;
        private readonly CongressDbContext _congressDbContext;

        public LegislationController(ILogger<LegislationController> logger, CongressDbContext dbContext)
        {
            _logger = logger;
            _congressDbContext = dbContext;
        }

        // Get count of active senators
        [HttpGet("getcount")]
        public async Task<ActionResult> GetCount()
        {
            int count = 0;
            try
            {
                count = await _congressDbContext.Legislations.CountAsync();

                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching count of senators: {ex}");
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
                var query = _congressDbContext.Legislations
                    .OrderByDescending(m => m.Number);

                var legislations = await query
                    .OrderByDescending(x=> Convert.ToInt32(x.Number))
                    .OrderByDescending(x => x.CongressId)
                    .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                var responseData = legislations.Select(m => new
                {
                    Id = m.Id,
                    Title = m.Title,
                    Number = m.Number,
                    Type = m.Type,
                    //Summaries = m.Summaries

                }).ToList();

                return Ok(responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching senators: {ex}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // Get specific senator details
        [HttpGet("{legislationId}")]
        public async Task<ActionResult> GetDetail(int legislationId)
        {
            if (legislationId <= 0)
            {
                return BadRequest("Invalid legislation id.");
            }

            try
            {
                var legislation = await _congressDbContext.Legislations
                    .FirstOrDefaultAsync(m => m.Id == legislationId);

                if (legislation == null)
                {
                    return NotFound("Legislation not found.");
                }


                var responseData = new
                {
                    Id = legislation.Id,
                    Title = legislation.Title,
                    Number = legislation.Number,
                    Type = legislation.Type,
                    Summaries = legislation.Summaries

                };

                
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching details for legislation {legislationId}: {ex}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpGet("getsponsoredlegislations/{memberId}")]
        public async Task<ActionResult> GetSponsoredLegislations(int memberId)
        {
            if (memberId <= 0)
            {
                return BadRequest("Invalid member id.");
            }

            try
            {

                var total = await _congressDbContext.Legislations
                   .Where(m => m.SponsoredLegislations
                   .Any(s => s.MemberId.Equals(memberId))).CountAsync();

                var legislations = await _congressDbContext.Legislations
                    .Where(m => m.SponsoredLegislations
                    .Any(s => s.MemberId.Equals(memberId)))
                    .ToListAsync();

                if (legislations == null)
                {
                    return NotFound("No Sponsored Legislations");
                }


                var responseData = legislations.Select(legislation => new
                {
                    Id = legislation.Id,
                    Title = legislation.Title,
                    Number = legislation.Number,
                    Type = legislation.Type,
                    Summaries = legislation.Summaries,
                }).ToList();


                return Ok(responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching sponsored legislations for member id: {memberId}: {ex}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpGet("getcosponsoredlegislations/{memberId}")]
        public async Task<ActionResult> GetCoSponsoredLegislations(int memberId)
        {
            if (memberId <= 0)
            {
                return BadRequest("Invalid member id.");
            }

            try
            {
                var total = await _congressDbContext.Legislations
                   .Where(m => m.CosponsoredLegislations
                   .Any(s => s.MemberId.Equals(memberId))).CountAsync();

                var legislations = await _congressDbContext.Legislations
                    .Where(m => m.CosponsoredLegislations
                    .Any(s => s.MemberId.Equals(memberId)))
                    .ToListAsync();

                if (legislations == null)
                {
                    return NotFound("No Sponsored Legislations");
                }


                var responseData = legislations.Select(legislation => new
                {
                    Id = legislation.Id,
                    Title = legislation.Title,
                    Number = legislation.Number,
                    Type = legislation.Type,
                    Summaries = legislation.Summaries,
                }).ToList();


                return Ok(responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching cosponsored legislations for member id: {memberId}: {ex}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
