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
        private readonly IUserService _userService;
        private readonly IPhotoManageService _photoService;

        public UserController(IUserService userService, IPhotoManageService photoService)
        {
            _userService = userService;
            _photoService = photoService;
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id, CancellationToken token)
        {
            var user = await _userService.GetUserById(id, token);
            if (user == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "User not found",
                    Detail = "Error occured while finding user on database"
                };
            }
            return StatusCode(StatusCodes.Status200OK, user);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateUser(CancellationToken token)
        {
            var user = new UserDTO()
            {
                Id = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),
                RoleName = HttpContext.User.FindFirstValue(ClaimTypes.Role),
                Email = HttpContext.User.FindFirstValue(ClaimTypes.Email)
            };
            if (user == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Title = "User didn't create",
                    Detail = "Error occured while creating user on server"
                };
            }

            var usr = await _userService.IsUserExists(user.Email);

            if (usr)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Title = "Email error",
                    Detail = "User with such email exists or doesn't find"
                };
            }
            if (await _userService.AddUser(user))
            {
                var createdUser = await _userService.GetUserByEmail(user.Email, token);
                return Created("api/user", createdUser);
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
        public async Task<IActionResult> UpdateUser([FromBody] UserDTO userDTO, CancellationToken token)
        {
            var userToUpdate = await _userService.GetUserById(userDTO.Id, token);

            if (userToUpdate == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "User not found",
                    Detail = $"User with {userDTO.Id} id not found"
                };
            }

            if (await _userService.UpdateUser(userDTO))
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
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id, CancellationToken token)
        {
            var userToDelete = await _userService.GetUserById(id, token);

            if (userToDelete == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "User not found",
                    Detail = $"User with {id} id not found"
                };
            }


            if (await _userService.DeleteUser(id))
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
