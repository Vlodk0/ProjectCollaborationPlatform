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
                return StatusCode(StatusCodes.Status404NotFound, "Project with such name doesn't exist");
            }
            return StatusCode(StatusCodes.Status404NotFound, project);
        }

        [HttpGet("projects")]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _projectService.GetAllProjects();
            if (projects != null || projects.Count != 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, projects);
            }
            return StatusCode(StatusCodes.Status404NotFound, "Projects don't exist");
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] ProjectDTO project)
        {
            try
            {
                if (project == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                var prj = await _projectService.GetProjectById(project.Id);

                if (prj == null)
                {
                    ModelState.AddModelError("id", "Project id already in use");
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState);
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
                    return StatusCode(StatusCodes.Status400BadRequest, "Project ID mismatch");
                }

                var projectToUpdate = await _projectService.GetProjectById(projectDTO.Id);

                if (projectToUpdate == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"Project with ID = {projectDTO.Id} not found");
                }

                if (await _projectService.UpdateProject(projectDTO))
                {
                    return StatusCode(StatusCodes.Status200OK, "Project updated succesfully");
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
                    return StatusCode(StatusCodes.Status404NotFound, $"Project with name = {name} not found");
                }

                if (await _projectService.DeleteProjectByName(name))
                {
                    return StatusCode(StatusCodes.Status200OK, "Project deleted succesfully");
                }
            }
            catch { }

            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error deleting data");
        }

    }
}
