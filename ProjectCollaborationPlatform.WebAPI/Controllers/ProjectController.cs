using Microsoft.AspNetCore.Mvc;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.Domain.DTOs;

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

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] ProjectDTO project)
        {
            try
            {
                if (project == null)
                {
                    return BadRequest();
                }

                var prj = await _projectService.GetProjectById(project.Id);

                if (prj == null)
                {
                    ModelState.AddModelError("id", "Project id already in use");
                    return BadRequest(ModelState);
                }
                if (await _projectService.AddProject(project))
                {
                    var createdProject = await _projectService.GetProjectByName(project.Title);
                    return CreatedAtAction(nameof(GetProjectByName), new { title = createdProject.Title }, createdProject);
                }


            }
            catch { }

            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProject([FromBody] ProjectDTO projectDTO)
        {
            try
            {
                if (projectDTO.Id != projectDTO.Id)
                {
                    return BadRequest("Project ID mismatch");
                }

                var projectToUpdate = await _projectService.GetProjectById(projectDTO.Id);

                if (projectToUpdate == null)
                {
                    return NotFound($"Project with ID = {projectDTO.Id} not found");
                }

                if (await _projectService.UpdateProject(projectDTO))
                {
                    return Ok("Project updated succesfully");
                }
            }
            catch { }

            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error updating data");
        }

        [HttpDelete("{name}")]
        public async Task<ActionResult<Project>> DeleteProjectByName([FromRoute] string name)
        {
            try
            {
                var projectToDelete = await _projectService.GetProjectByName(name);

                if (projectToDelete == null)
                {
                    return NotFound($"Project with name = {name} not found");
                }

                if (await _projectService.DeleteProjectByName(name))
                {
                    return Ok("Project deleted succesfully");
                }
            }
            catch { }

            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error deleting data");
        }

    }
}
