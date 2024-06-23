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
    [Route("api/scorecard")]
    public class ScoreCardController : ControllerBase
    {
        private readonly ILogger<ScoreCardController> _logger;
        private readonly CongressDbContext _congressDbContext;

        public ScoreCardController(ILogger<ScoreCardController> logger, CongressDbContext dbContext)
        {
            _logger = logger;
            _congressDbContext = dbContext;
        }

        [HttpGet("getall")]
        public Task<ActionResult> GetAll(int page = 1)
        {
            throw new NotImplementedException();
        }

        [HttpGet("getscorecards/{memberId}")]
        public async Task<ActionResult> GetScoreCards(int memberId)
        {
            if (memberId <= 0)
            {
                return BadRequest("Invalid member id.");
            }

            try
            {
                var total = await _congressDbContext.ScoreCards
                    .Where(m => m.MemberId.Equals(memberId)).CountAsync();

                var scoreCards = await _congressDbContext.ScoreCards
                    .Where(m => m.MemberId.Equals(memberId)).ToListAsync();

                if (scoreCards == null)
                {
                    return NotFound("No Score Cards");
                }


                var responseData = scoreCards.Select(sc => new
                {
                    Subject = sc.Subject.Name,
                    Score = sc.Score,
                    Comment = sc.Comment
                }).ToList();


                return Ok(responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching score cards for member id: {memberId}: {ex}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("getcount")]
        public Task<ActionResult> GetCount()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{memberId}")]
        public Task<ActionResult> GetDetail(int memberId)
        {
            throw new NotImplementedException();
        }
    }
}
