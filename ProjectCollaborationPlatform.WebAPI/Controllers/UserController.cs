using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.Domain.DTOs;
using System.Security.Claims;

namespace ProjectCollaborationPlatform.WebAPI.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id, CancellationToken token)
        {
            var user = await _userService.GetUserById(id, token);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "User doesn't exist");
            }
            return StatusCode(StatusCodes.Status400BadRequest, user);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers(CancellationToken token)
        {
            var users = await _userService.GetAllUsers(token);
            if (users != null || users.Count != 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, users);
            }
            return StatusCode(StatusCodes.Status404NotFound, "User doesn't exist");

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateUser(CancellationToken token)
        {
            try
            {
                var user = new UserDTO()
                {
                    Id = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    RoleName = HttpContext.User.FindFirstValue(ClaimTypes.Role),
                    Email = HttpContext.User.FindFirstValue(ClaimTypes.Email)
                };
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                var usr = await _userService.GetUserByEmail(user.Email, token);

                if(usr != null)
                {
                    ModelState.AddModelError("email", "User email already in use");
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState);
                }
                if (await _userService.AddUser(user))
                {
                    var createdUser = await _userService.GetUserByEmail(user.Email, token);
                    return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
                }

                
            }
            catch { }

            return StatusCode(StatusCodes.Status500InternalServerError,
                "Server Error");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserDTO userDTO, CancellationToken token)
        {
            try
            {
                if (userDTO.Id != userDTO.Id)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "User Id mismatch");
                }

                var userToUpdate = await _userService.GetUserById(userDTO.Id, token);

                if (userToUpdate == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"User with ID = {userDTO.Id} not found");
                }

                if (await _userService.UpdateUser(userDTO))
                {
                    return StatusCode(StatusCodes.Status200OK, "User succesfully updated");
                }

            }
            catch { }

            return StatusCode(StatusCodes.Status500InternalServerError,
                "Server Error");
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id, CancellationToken token)
        {
            try
            {
                var userToDelete = await _userService.GetUserById(id, token);

                if (userToDelete == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"User with ID = {id} not found");
                }

                
                if (await _userService.DeleteUser(id))
                {
                    return StatusCode(StatusCodes.Status200OK, "User succesfully deleted");
                }

            }
            catch { }

            return StatusCode(StatusCodes.Status500InternalServerError,
                "Server Error");
        }

    }
}
