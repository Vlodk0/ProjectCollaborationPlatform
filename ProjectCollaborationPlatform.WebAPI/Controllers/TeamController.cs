using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.BL.Services;
using ProjectCollaborationPlatform.DAL.Data.Models;
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

        //[Authorize]
        [HttpPost("teams")]
        public async Task<IActionResult> AddUsersToTeam(Guid id, List<TeamDeveloperIdDTO> developerIdDTO)
        {
            var result = await _teamService.AddUsersToTeam(id, developerIdDTO);

            if (result)
            {
                return Ok();
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

        //[Authorize]
        [HttpGet("teams")]
        public async Task<IActionResult> GetAllTeams()
        {
            var result = await _teamService.GetTeamsWithDevelopers();

            if(result != null && result.Count > 0)
            {
                return Ok(result);
            }
            else
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Teams not found",
                    Detail = "Error occured while finding teams"
                };
            }
        }

        //[Authorize]
        [HttpDelete]
        // Make TeamMemberController for make action with team and users, NOT CRUD
        public async Task<IActionResult> DeleteUserFromTeam(Guid id, List<TeamDeveloperIdDTO> developerIdDTO, CancellationToken token)
        {
            var team = await _teamService.GetTeamById(id, token);

            if (team == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Team not found",
                    Detail = "Team with such Id doesn't exist"
                };
            }

            var result = await _teamService.UpdateTeam(id, developerIdDTO);

            if (result)
            {
                return Ok(result);
            }
            else
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Title = "Server error",
                    Detail = "Error occured while server started"
                };
            }

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateTeam([FromBody] TeamDTO teamDTO, CancellationToken token)
        {
            if (teamDTO == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Title = "Bad request",
                    Detail = "Can't create a board"
                };
            }

            var team = await _teamService.GetTeamById(teamDTO.Id, token);

            if (team != null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Title = "Error creating the team",
                    Detail = "Board is already in use"
                };
            }
            if (await _teamService.CreateTeam(teamDTO))
            {
                var createdProject = await _teamService.GetTeamById(teamDTO.Id, token);
                return Created("api/team", createdProject);
            }
            else
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Title = "Server Error",
                    Detail = "Error occured while server running"
                };
            }
        }
    }
}
