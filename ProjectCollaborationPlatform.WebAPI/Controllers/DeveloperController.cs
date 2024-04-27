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
    public class DeveloperController : ControllerBase
    {
        private readonly IDeveloperService _developerService;
        private readonly IPhotoManageService _photoService;

        public DeveloperController(IDeveloperService developerService, IPhotoManageService photoService)
        {
            _photoService = photoService;
            _developerService = developerService;
        }

        [Authorize(Policy = "DeveloperRole")]
        [HttpPost]
        public async Task<IActionResult> CreateDeveloper(CreateDeveloperDTO developerDTO, CancellationToken token)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Guid id = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            string email = HttpContext.User.FindFirstValue(ClaimTypes.Email);

            var usr = await _developerService.GetDeveloperById(id, token);

            if (usr != null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Title = "Bad request",
                    Detail = "User with such id exists or doesn't find"
                };
            }

            var user = new DeveloperDTO()
            {
                Id = id,
                FirstName = developerDTO.FirstName,
                LastName = developerDTO.LastName,
                Email = email
            };

            var createUser = await _developerService.AddDeveloper(id, user);

            if (createUser)
            {
                return Ok(new
                {
                    message = "User created"
                });
            }
            else
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Title = "Server error",
                    Detail = "Error occured while server running"
                };
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetDeveloperById(CancellationToken token)
        {
            Guid id = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var projectOwner = await _developerService.GetDeveloperById(id, token);

            if (projectOwner == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "User not found",
                    Detail = $"User with such id not found"
                };
            }

            return Ok(new
            {
                message = "User exists"
            });

        }

        [Authorize]
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetDeveloper([FromRoute] Guid id, CancellationToken token)
        {
            var dev = await _developerService.GetDeveloperById(id, token);

            if (dev == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "User not found",
                    Detail = "User with such id not found"
                };
            }

            return Ok(dev);

        }

        [Authorize(Policy = "AdminRole")]
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteDeveloper([FromRoute] Guid id, CancellationToken token)
        {
            var userToDelete = await _developerService.GetDeveloperById(id, token);

            if (userToDelete == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "User not found",
                    Detail = "User with such id not found"
                };
            }


            if (await _developerService.DeleteDeveloper(id))
            {
                return NoContent();
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
        [HttpGet("developers")]
        public async Task<IActionResult> GetAllDevelopers([FromQuery] PaginationFilter paginationFilter, CancellationToken token)
        {
            var filter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
            var developers = await _developerService.GetAllDevelopers(filter, token);

            return Ok(developers);
        }
    }
}
