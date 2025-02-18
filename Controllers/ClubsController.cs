using System;
using System.Threading.Tasks;
using Clubs.Data;
using Clubs.Models;
using Clubs.DTOs;
using Clubs.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clubs.Controllers
{
    [ApiController]
    [Route("api/clubs")]
    public class ClubsController : BaseController
    {
        private readonly IClubService _clubService;

        public ClubsController(IClubService clubService)
        {
            _clubService = clubService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClub([FromBody] ClubRequestDto clubDto)
        {
            if (PlayerId == null || PlayerId <= 0)
                return BadRequest("Player-ID header is required and must be a valid integer.");

            try
            {
                var existingClub = await _clubService.ClubExistsAsync(clubDto.Name);
                if (existingClub != false)
                {
                    return Conflict("A club with the same name already exists.");
                }

                // Create the club and add the Player-ID as a member/owner.
                var createdClub = await _clubService.CreateClubAsync(clubDto, PlayerId.Value);
                return CreatedAtAction(nameof(CreateClub),
                    createdClub);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet("{clubId}")]
        public async Task<IActionResult> GetClubById(Guid clubId)
        {
            var club = await _clubService.GetClubByIdAsync(clubId);
            if (club == null)
                return NotFound();
            return Ok(club);
        }

        [HttpPost("{clubId}/members")]
        public async Task<IActionResult> AddMember(Guid clubId, [FromBody] PlayerRequestDto player)
        {
            if (PlayerId == null || PlayerId <= 0)
                return BadRequest("Player-ID header is required and must be a valid integer.");

            await _clubService.AddMemberAsync(clubId, player.PlayerId);
            return NoContent();
        }
    }
}
