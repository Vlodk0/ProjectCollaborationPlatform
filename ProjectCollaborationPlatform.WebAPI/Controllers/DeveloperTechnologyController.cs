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
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Dev not found",
                    Detail = "Dev with such Id doesn't exist"
                };
            }
        }
    }
}
