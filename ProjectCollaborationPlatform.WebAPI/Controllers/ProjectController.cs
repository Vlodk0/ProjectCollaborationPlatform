using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.BL.Services;
using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.Domain.DTOs;
using ProjectCollaborationPlatform.Domain.Helpers;
using ProjectCollaborationPlatform.Domain.Pagination;
using System.Security.Claims;

namespace ProjectCollaborationPlatform.WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ITechnologyService _technologyService;
        private readonly IDeveloperService _developerService;

        public ProjectController(IProjectService projectService, ITechnologyService technologyService, IDeveloperService developerService)
        {
            _projectService = projectService;
            _technologyService = technologyService;
            _developerService = developerService;
        }

        [Authorize]
        [HttpGet("{name}")]
        public async Task<IActionResult> GetProjectByName([FromRoute] string name, CancellationToken token)
        {
            var project = await _projectService.GetProjectByName(name, token);
            if (project == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Project not found",
                    Detail = "Project with such name doesn't exist"
                };
            }
            return Ok(project);
        }

        [Authorize]
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetProjectById([FromRoute] Guid id, CancellationToken token)
        {
            var project = await _projectService.GetProjectById(id, token);
            if (project == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Project not found",
                    Detail = "Project with such id doesn't exist"
                };
            }
            return Ok(project);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects([FromQuery] int pageNumber, [FromQuery] int pageSize,
            [FromQuery] string sortColumn, [FromQuery] string sortDirection, CancellationToken token)
        {
            var filter = new PaginationFilter(pageNumber, pageSize, sortColumn, sortDirection);
            var projects = await _projectService.GetAllProjects(filter, token);

            return Ok(projects);
        }

        [HttpGet("my-projects")]
        public async Task<IActionResult> GetAllProjectsByProjectOwnerId([FromQuery] int pageNumber, [FromQuery] int pageSize,
    [FromQuery] string sortColumn, [FromQuery] string sortDirection, CancellationToken token)
        {
            Guid projOwnerId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var filter = new PaginationFilter(pageNumber, pageSize, sortColumn, sortDirection);
            var projects = await _projectService.GetAllProjectsByProjectOwnerId(projOwnerId, filter, token);

            return Ok(projects);
        }

        [HttpGet("projectOwner/projects")]
        public async Task<IActionResult> GetProjectOwnerProjects(CancellationToken token)
        {
            Guid projOwnerId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var projects = await _projectService.GetProjectOwnerListProjects(projOwnerId, token);
            if (projects == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Project not found",
                    Detail = "Project with such id doesn't exist"
                };
            }
            return Ok(projects);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDTO project, CancellationToken token)
        {
            Guid id = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (project == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Title = "Bad request",
                    Detail = "Can't create a project"
                };
            }

            var prj = await _projectService.GetProjectByName(project.Title, token);

            if (prj != null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Title = "Error creating project",
                    Detail = "Project is already exist"
                };
            }
            if (await _projectService.AddProject(project, id, token))
            {
                var createdProject = await _projectService.GetProjectByName(project.Title, token);
                return Created("api/project", createdProject);
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

        [Authorize]
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateProject([FromBody] CreateProjectDTO projectDTO, [FromRoute] Guid id, CancellationToken token)
        {

            var projectToUpdate = await _projectService.GetProjectById(id, token);

            if (projectToUpdate == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Projects not found",
                    Detail = "Project with such id not found"
                };
            }

            if (await _projectService.UpdateProject(projectDTO, id))
            {
                return Ok("Project updated succesfully");
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

        [Authorize]
        [HttpPut("ProjectDetails/{id:Guid}")]
        public async Task<IActionResult> UpdateProjectDetails([FromBody] ProjectDetailDTO projectDetailDTO, [FromRoute] Guid id, CancellationToken token)
        {

            if (await _projectService.UpdateProjectDetails(id, projectDetailDTO.Description))
            {
                return StatusCode(StatusCodes.Status200OK, "Project details updated succesfully");
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

        [Authorize]
        [HttpDelete("{name}")]
        public async Task<ActionResult<Project>> DeleteProjectByName([FromRoute] string name, CancellationToken token)
        {
            var projectToDelete = await _projectService.GetProjectByName(name, token);

            if (projectToDelete == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Projects not found",
                    Detail = "Project with such name not found"
                };
            }

            if (await _projectService.DeleteProjectByName(name, token))
            {
                return Ok("Project deleted successfully");
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

        [Authorize]
        [HttpPost("technologies/{id:Guid}")]
        public async Task<IActionResult> AddTechnologyToProject([FromRoute] Guid id, List<string> techId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _technologyService.AddTechnologyForProject(id, techId);

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
                    Detail = "Error occured while adding technologies for project"
                };
            }
        }

        //[Authorize]
        [HttpPost("developers/{id:Guid}")]
        public async Task<IActionResult> AddDevelopersToProject([FromRoute] Guid id, [FromBody] List<Guid> developerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _developerService.AddDeveloperForProject(id, developerId);

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
                    Detail = "Error occured while adding developers for project"
                };
            }
        }

        [Authorize]
        [HttpDelete("technologies/{id:Guid}")]
        public async Task<IActionResult> RemoveTechnologiesFromProject([FromRoute] Guid id, List<string> techId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _technologyService.RemoveTechnologyFromProject(id, techId);

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
                    Detail = "Error occured while removing technologies from project"
                };
            }
        }

        [Authorize]
        [HttpDelete("developers/{id:Guid}")]
        public async Task<IActionResult> RemoveDevelopersFromProject([FromRoute] Guid id, List<Guid> developerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _developerService.RemoveDeveloperFromProject(id, developerId);

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
                    Detail = "Error occured while removing developers from project"
                };
            }
        }
    }
}

