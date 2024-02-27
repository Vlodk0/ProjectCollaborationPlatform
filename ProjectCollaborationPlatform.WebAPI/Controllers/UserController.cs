using Microsoft.AspNetCore.Mvc;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.Domain.DTOs;

namespace ProjectCollaborationPlatform.WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "User doesn't exist");
            }
            return StatusCode(StatusCodes.Status400BadRequest, user);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            if (users != null || users.Count != 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, users);
            }
            return StatusCode(StatusCodes.Status404NotFound, "User doesn't exist");

        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO user)
        {
            try
            {
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                var usr = await _userService.GetUserByEmail(user.Email);

                if(usr == null)
                {
                    ModelState.AddModelError("email", "User email already in use");
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState);
                }
                if (await _userService.AddUser(user))
                {
                    var createdUser = await _userService.GetUserByEmail(user.Email);
                    return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
                }

                
            }
            catch { }

            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error deleting data");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserDTO userDTO)
        {
            try
            {
                if (userDTO.Id != userDTO.Id)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "User Id mismatch");
                }

                var userToUpdate = await _userService.GetUserById(userDTO.Id);

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
                "Error deleting data");
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            try
            {
                var userToDelete = await _userService.GetUserById(id);

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
                "Error deleting data");
        }

    }
}
