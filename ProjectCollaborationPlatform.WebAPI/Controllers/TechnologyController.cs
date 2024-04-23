using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.Domain.Helpers;

namespace ProjectCollaborationPlatform.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnologyController : ControllerBase
    {
        private readonly ITechnologyService _technologyService;

        public TechnologyController(ITechnologyService technologyService)
        {
            _technologyService = technologyService;
        }

        [Authorize(Policy = "DeveloperRole")]
        [HttpGet]
        public async Task<IActionResult> GetAllTechnologies(CancellationToken token)
        {
            var technologies = await _technologyService.GetAllTechnologies(token);

            if (technologies == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Technologies not found",
                    Detail = "Technologies with such id doesn't exist"
                };
            }
            return Ok(technologies);
        }

        [Authorize]
        [HttpGet("{projId:Guid}")]
        public async Task<IActionResult> GetAllProjectTechnologies([FromRoute] Guid projId, CancellationToken token)
        {
            var technologies = await _technologyService.GetAllTechnologiesByProjectId(projId, token);

            if (technologies == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Technologies not found",
                    Detail = "Technologies with such id doesn't exist"
                };
            }
            return Ok(technologies);
        }
    }
}
