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
    public class DeveloperController : ControllerBase
    {
        private readonly IDeveloperService _developerService;
        private readonly IPhotoManageService _photoService;

        public DeveloperController(IDeveloperService developerService, IPhotoManageService photoService)
        {
            _photoService = photoService;
            _developerService = developerService;
        }

        [Authorize]
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
                return Ok(createUser);
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> FileDownload([FromQuery] string fileName)
        {
            var result = await _photoService.DownloadFile(fileName);
            return File(result.Item1, result.Item2, result.Item3);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDeveloper([FromBody] DeveloperDTO developerDTO, CancellationToken token)
        {
            var userToUpdate = await _developerService.GetDeveloperById(developerDTO.Id, token);

            if (userToUpdate == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "User not found",
                    Detail = $"User with {developerDTO.Id} id not found"
                };
            }

            if (await _developerService.UpdateDeveloper(developerDTO))
            {
                return StatusCode(StatusCodes.Status200OK, "User succesfully updated");
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
                    Detail = $"User with {id} id not found"
                };
            }


            if (await _developerService.DeleteDeveloper(id))
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
    }
}
