using Microsoft.AspNetCore.Mvc;
using ProjectCollaborationPlatform.BL.Interfaces;

namespace ProjectCollaborationPlatform.WebAPI.Controllers
{

    [Route("api/[contoller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetUserById([FromQuery] Guid id)
        {
            var user = await _userService.GetById(id);
            if(user == null)
            {
                return NotFound("User doesn't exist");
            }
            return Ok(user);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            if (users != null || users.Count != 0)
            {
                return Ok(users);
            }
            return NotFound("User doesn't exist");
        }
    }
}
