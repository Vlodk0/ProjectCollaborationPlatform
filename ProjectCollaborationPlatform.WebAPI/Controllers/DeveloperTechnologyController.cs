using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.Domain.DTOs;
using ProjectCollaborationPlatform.Domain.Helpers;
using System.Security.Claims;

namespace ProjectCollaborationPlatform.WebAPI.Controllers
{
    [Route("api/Developer/Technologies")]
    [ApiController]
    public class DeveloperTechnologyController : ControllerBase
    {
        private readonly ITechnologyService _technologyService;

        public DeveloperTechnologyController(ITechnologyService technologyService)
        {
            _technologyService = technologyService;
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddTechnologyForDeveloper([FromBody] List<string> techId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Guid id = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _technologyService.AddTechnologyForDeveloper(id, techId);

            if (result)
            {
                return Ok();
            }
            else
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Title = "Server Error",
                    Detail = "Error occured while adding technologies for dev"
                };
            }
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> RemoveTechnologyFromDeveloper([FromBody] List<string> techId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Guid id = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _technologyService.RemoveTechnologyFromDeveloper(id, techId);

            if (result)
            {
                return Ok();
            }
            else
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Title = "Server Error",
                    Detail = "Error occured while deleting from dev"
                };
            }
        }
    }
}
