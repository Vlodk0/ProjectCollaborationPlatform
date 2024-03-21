using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCollaborationPlatform.DAL.Data.DataAccess;
using ProjectCollaborationPlatform.Domain.Helpers;
using System.Security.Claims;

namespace ProjectCollaborationPlatform.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ProjectPlatformContext _context;

        public UserController(ProjectPlatformContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetById()
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
                return Redirect($"/api/Developer/get");
            }
            else if (roleName == "ProjectOwner")
            {
                return Redirect($"/api/ProjectOwner/get");
            }
            return BadRequest();
        }
    }
}
