using Microsoft.AspNetCore.Mvc;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.Domain.DTOs;
using ProjectCollaborationPlatform.Domain.Helpers;

namespace ProjectCollaborationPlatform.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpPost("teams/{id:Guid}")]
        public async Task<IActionResult> AddUsersToTeam(Guid id, List<TeamDeveloperIdDTO> developerIdDTO)
        {
            var result = await _teamService.AddUsersToTeam(id, developerIdDTO);

            if(result)
            {
                return Ok(result);
            }
            else
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Team not found",
                    Detail = "Team with such Id doesn't exist"
                };
            }
        }
    }
}
