using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.BL.Services;
using ProjectCollaborationPlatform.Domain.DTOs;
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

        [Authorize]
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

        [Authorize]
        [HttpGet("statistic")]
        public async Task<ActionResult<List<CountTechnologyOnProjectsDTO>>> GetTechnologyStatisticByProjects(CancellationToken token)
        {
            try
            {
                var technologyStats = await _technologyService.GetTechnologyStatisticByProjects(token);
                return Ok(technologyStats);
            }
            catch (Exception ex)
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
