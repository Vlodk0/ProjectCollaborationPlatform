using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.WebAPI.Interfaces;

namespace ProjectCollaborationPlatform.WebAPI.Controllers
{

    [Route("api/[contoller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        [HttpGet("Get")]
        public async Task<IActionResult> Get([FromQuery] Guid id)
        {
            var user = await _userService.Get(id);
            if(user == null)
            {
                return NotFound("User doesn't exist");
            }
            return Ok(user);
        }
    }
}
