using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.BL.Services;
using ProjectCollaborationPlatform.Domain.DTOs;
using ProjectCollaborationPlatform.Domain.Helpers;
using ProjectCollaborationPlatform.Domain.Pagination;
using System.Security.Claims;

namespace ProjectCollaborationPlatform.WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProjectOwnerController : ControllerBase
    {
        private readonly IProjectOwnerService _projectOwnerService;
        private readonly IPhotoManageService _photoService;

        public ProjectOwnerController(IProjectOwnerService projectOwnerService, IPhotoManageService photoManageService)
        {
            _projectOwnerService = projectOwnerService;
            _photoService = photoManageService;
        }

        [Authorize(Policy = "ProjectOwnerRole")]
        [HttpPost]
        public async Task<IActionResult> CreateProjectOwner(CreateProjectOwnerDTO projectOwnerDTO, CancellationToken token)
            {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Guid id = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            string email = HttpContext.User.FindFirstValue(ClaimTypes.Email);

            var usr = await _projectOwnerService.GetProjectOwnerById(id, token);

            if (usr != null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Title = "Bad request",
                    Detail = "User with such id exists or doesn't find"
                };
            }

            var user = new ProjectOwnerDTO()
            {
                Id = id,
                FirstName = projectOwnerDTO.FirstName,
                LastName = projectOwnerDTO.LastName,
                Email = email,
            };

            var createUser = await _projectOwnerService.AddProjectOwner(id, user);

            if (createUser)
            {
                return Ok();
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

        [Authorize(Policy = "ProjectOwnerRole")]
        [HttpGet]
        public async Task<IActionResult> GetProjectOwnerById(CancellationToken token)
        {
            Guid id = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var projectOwner = await _projectOwnerService.GetProjectOwnerById(id, token);

            if(projectOwner == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "User not found",
                    Detail = $"User with such id not found"
                };
            }

            return Ok(projectOwner);

        }

        [Authorize(Policy = "ProjectOwnerRole")]
        [HttpPost("photo")]
        public async Task<IActionResult> PhotoUpload(IFormFile formFile)
        {
            Guid userParseId;
            try
            {
                userParseId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            catch (Exception)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status422UnprocessableEntity,
                    Title = "Something wrong with user Guid",
                    Detail = "Error occured while parsing guid from user claims"
                };
            }
            var result = await _photoService.UploadFile(formFile, userParseId);

            return Ok(result);
        }


        [Authorize(Policy = "ProjectOwnerRole")]
        [HttpGet("file")]
        public async Task<IActionResult> FileDownload([FromQuery] string fileName)
        {
            var result = await _photoService.DownloadFile(fileName);
            return File(result.Item1, result.Item2, result.Item3);
        }

        [Authorize(Policy = "AdminRole")]
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteProjectOwner([FromRoute] Guid id, CancellationToken token)
        {
            var userToDelete = await _projectOwnerService.GetProjectOwnerById(id, token);

            if (userToDelete == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "User not found",
                    Detail = $"User with {id} id not found"
                };
            }


            if (await _projectOwnerService.DeleteProjectOwner(id))
            {
                return StatusCode(StatusCodes.Status200OK, "User succesfully deleted");
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

        [Authorize(Policy = "AdminRole")]
        [HttpGet("projectOwners")]
        public async Task<IActionResult> GetAllProjectOwners([FromQuery] PaginationFilter paginationFilter, CancellationToken token)
        {
            var filter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
            var developers = await _projectOwnerService.GetAllProjectOwners(filter, token);

            return Ok(developers);
        }

    }
}
