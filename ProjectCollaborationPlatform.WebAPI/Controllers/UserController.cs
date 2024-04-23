using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.Domain.DTOs;
using ProjectCollaborationPlatform.Domain.Helpers;
using System.Security.Claims;

namespace ProjectCollaborationPlatform.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IDeveloperService _developerService;
        private readonly IProjectOwnerService _projectOwnerService;


        public UserController(IDeveloperService developerService, IProjectOwnerService projectOwnerService)
        {
            _developerService = developerService;
            _projectOwnerService = projectOwnerService;
        }



        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetById(CancellationToken token)
        {
            Guid id;
            String roleName;
            try
            {
                id = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                roleName = HttpContext.User.FindFirstValue(ClaimTypes.Role);
            }
            catch (Exception)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status422UnprocessableEntity,
                    Title = "Error parsing claims",
                    Detail = "Error occured while parsing claims on server"
                };
            }

            if (roleName == "Dev")
            {
                var dev = await _developerService.GetDeveloperById(id, token);
                if (dev != null)
                {
                    return Ok(dev);
                }
                else
                {
                    throw new CustomApiException()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Title = "Not found",
                        Detail = "Dev with such id is not found"
                    };
                }
            }
            else if (roleName == "ProjectOwner")
            {
                var projOwner = await _projectOwnerService.GetProjectOwnerById(id, token);
                if (projOwner != null)
                {
                    return Ok(projOwner);
                }
                else
                {
                    throw new CustomApiException()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Title = "Not found",
                        Detail = "Project Owner with such id is not found"
                    };
                }
            }
            return BadRequest();
        }

        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO userDTO, CancellationToken token)
        {
            Guid id;
            String roleName;
            try
            {
                id = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                roleName = HttpContext.User.FindFirstValue(ClaimTypes.Role);
            }
            catch (Exception)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status422UnprocessableEntity,
                    Title = "Error parsing claims",
                    Detail = "Error occured while parsing claims on server"
                };
            }

            if (roleName == "Dev")
            {
                var dev = await _developerService.UpdateDeveloper(id, userDTO);
                if (dev)
                {
                    return Ok(dev);
                }
                else
                {
                    throw new CustomApiException()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Title = "Not found",
                        Detail = "Dev with such id is not found"
                    };
                }
            }
            else if (roleName == "ProjectOwner")
            {
                var projOwner = await _projectOwnerService.UpdateProjectOwner(id, userDTO);
                if (projOwner)
                {
                    return Ok(projOwner);
                }
                else
                {
                    throw new CustomApiException()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Title = "Not found",
                        Detail = "Project Owner with such id is not found"
                    };
                }
            }
            return BadRequest();
        }
    }
}
