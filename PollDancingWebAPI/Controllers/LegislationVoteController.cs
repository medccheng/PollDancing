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
    [Route("api/legislationvote")]
    public class LegislationVoteController : ControllerBase, IMember
    {
        private readonly ILogger<LegislationVoteController> _logger;
        private readonly CongressDbContext _congressDbContext;

        public LegislationVoteController(ILogger<LegislationVoteController> logger, CongressDbContext dbContext)
        {
            _logger = logger;
            _congressDbContext = dbContext;
        }

        [HttpGet("getall")]
        public Task<ActionResult> GetAll(int page = 1)
        {
            throw new NotImplementedException();
        }

        [HttpGet("getbillvotes/{memberId}")]
        public async Task<ActionResult> GetBillVotes(int memberId)
        {
            if (memberId <= 0)
            {
                return BadRequest("Invalid member id.");
            }

            try
            {
                var total = await _congressDbContext.MemberLegislationVotes
                    .Where(m => m.MemberId.Equals(memberId)).CountAsync();

                var votes = await _congressDbContext.MemberLegislationVotes
                    .Where(m => m.MemberId.Equals(memberId)).ToListAsync();

                if (votes == null)
                {
                    return NotFound("No Bill Votes");
                }


                var responseData = votes.Select(vote => new
                {
                    Title = vote.Legislation.Title,
                    Vote = vote.Vote,
                }).ToList();


                return Ok(responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching sponsored legislations for member id: {memberId}: {ex}");
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
