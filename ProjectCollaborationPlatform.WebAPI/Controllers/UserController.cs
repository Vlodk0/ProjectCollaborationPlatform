using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.Domain.DTOs;
using ProjectCollaborationPlatform.Domain.Helpers;
using ProjectCollaborationPlatform.Domain.Pagination;
using System.Security.Claims;

namespace ProjectCollaborationPlatform.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IDeveloperService _developerService;
        private readonly IProjectOwnerService _projectOwnerService;
        private readonly IProjectService _projectService;
        private readonly IPhotoManageService _photoService;


        public UserController(IDeveloperService developerService, IProjectOwnerService projectOwnerService, IProjectService projectService, IPhotoManageService photoService)
        {
            _developerService = developerService;
            _projectOwnerService = projectOwnerService;
            _projectService = projectService;
            _photoService = photoService;
        }



        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetById(CancellationToken token)// a lot of business logic inside of controller. consider extracting into the service
        //also there shouldn't be any difference between dev and PO - they are both users. common logic must be extracted into base entity.
        //according to your BL PO can be a user with 1 assigned project and without technologies, so it can be even single entity
        //all repeatable logic with user types will become redundant
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
            else if (roleName == "ProjectOwner" || roleName == "Admin")
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
        [HttpGet("currentUser")]
        public async Task<IActionResult> IsUserExists(CancellationToken token)
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
                        Title = "Dev not found",
                        Detail = "Dev with such id is not found"
                    };
                }
            }
            else if (roleName == "ProjectOwner" || roleName == "Admin")
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
                        Title = "Project Owner not found",
                        Detail = "Project Owner with such id is not found"
                    };
                }
            }
            return BadRequest();
        }


        [Authorize]
        [HttpGet("user")]
        public async Task<IActionResult> GetByIdWithAvatar(CancellationToken token)
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
                var dev = await _developerService.GetDeveloperWithAvatar(id, token);
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
            else if (roleName == "ProjectOwner" || roleName == "Admin")
            {
                var projOwner = await _projectOwnerService.GetProjectOwnerWithAvatar(id, token);
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
                    return NoContent();
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
                    return NoContent();
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
        [HttpGet("projects")]
        public async Task<IActionResult> GetAllUserProjects([FromQuery] int pageNumber, [FromQuery] int pageSize,
                                        [FromQuery] string sortColumn, [FromQuery] string sortDirection, CancellationToken token)
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

            var filter = new PaginationFilter(pageNumber, pageSize, sortColumn, sortDirection);

            if (roleName == "Dev")
            {
                var projects = await _projectService.GetAllProjectsWhereDevsExists(id, filter, token);
                if (projects != null)
                {
                    return Ok(projects);
                }
                else
                {
                    throw new CustomApiException()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Title = "Not found",
                        Detail = "Projects not found"
                    };
                }
            }
            else if (roleName == "ProjectOwner")
            {
                var projOwner = await _projectService.GetAllProjectsByProjectOwnerId(id, filter, token);
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
                        Detail = "Projects not found"
                    };
                }
            }
            return BadRequest();
        }

        [Authorize]
        [HttpPost("photo")]
        public async Task<IActionResult> PhotoUpload(IFormFile file)
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

            var result = await _photoService.UploadFile(file, userParseId);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{fileName}")]
        public async Task<IActionResult> FileDownload([FromRoute] string fileName)
        {
            var result = await _photoService.DownloadFile(fileName);
            return File(result.Item1, result.Item2, result.Item3);
        }
    }
}
