using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCollaborationPlatform.BL.Interfaces;
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

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

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
            return StatusCode(StatusCodes.Status200OK, project);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects([FromQuery] int pageNumber, [FromQuery] int pageSize,
            [FromQuery] string sortColumn, [FromQuery] string sortDirection, CancellationToken token)
        {
            var filter = new PaginationFilter(pageNumber, pageSize, sortColumn, sortDirection);
            var projects = await _projectService.GetAllProjects(filter, token);

            return Ok(projects);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] ProjectFullInfoDTO project, CancellationToken token)
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
        public async Task<IActionResult> UpdateProject([FromBody] ProjectDTO projectDTO, [FromRoute] Guid id, CancellationToken token)
        {

            var projectToUpdate = await _projectService.GetProjectById(id, token);

            if (projectToUpdate == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Projects not found",
                    Detail = $"Project with id not found"
                };
            }

            if (await _projectService.UpdateProject(projectDTO, id))
            {
                return StatusCode(StatusCodes.Status200OK, "Project updated succesfully");
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
        public async Task<IActionResult> UpdateProjectDetails([FromBody] ProjectDetailsDTO projectDTO, [FromRoute] Guid id, CancellationToken token)
        {

            if (await _projectService.UpdateProjectDetails(id, projectDTO.Description))
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
                    Detail = $"Project with such name: {name}, not found"
                }; ;
            }

            if (await _projectService.DeleteProjectByName(name))
            {
                return StatusCode(StatusCodes.Status200OK, "Project deleted succesfully");
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

