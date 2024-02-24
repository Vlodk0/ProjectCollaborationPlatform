using Microsoft.AspNetCore.Mvc;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.BL.Services;
using ProjectCollaborationPlatform.DAL.Data.Models;

namespace ProjectCollaborationPlatform.WebAPI.Controllers
{

    [Route("api/[contoller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet("{name}:string")]
        public async Task<IActionResult> GetProjectByName([FromRoute] string name)
        {
            var project = await _projectService.GetProjectByName(name);
            if (project == null)
            {
                return NotFound("Project with such name doesn't exist");
            }
            return Ok(project);
        }

        [HttpGet("projects")]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _projectService.GetAllProjects();
            if (projects != null || projects.Count != 0)
            {
                return Ok(projects);
            }
            return NotFound("Projects don't exist");
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateProject(Guid id, Project project)
        {
            try
            {
                if (id != project.Id)
                {
                    return BadRequest("Project ID mismatch");
                }

                var userToUpdate = await _projectService.GetProjectById(id);

                if (userToUpdate == null)
                {
                    return NotFound($"Project with ID = {id} not found");
                }

                if (await _projectService.UpdateProject(project))
                {
                    return Ok("Project updated succesfully");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                  "Error updating data");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Error updating data");
            }
        }

        [HttpDelete("{name:string}")]
        public async Task<ActionResult<Project>> DeleteProjectByName(string name)
        {
            try
            {
                var userToDelete = await _projectService.GetProjectByName(name);

                if (userToDelete == null)
                {
                    return NotFound($"Project with name = {name} not found");
                }

                if (await _projectService.DeleteProjectByName(name))
                {
                    return Ok("Project deleted succesfully");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        "Error deleting data");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }

    }
}
