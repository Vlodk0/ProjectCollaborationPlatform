using System.Security.Claims;
using ProjectCollaborationPlatform.Domain.Helpers;

namespace ProjectCollaborationPlatform.WebAPI.Helpers;

public static class TokenClaimsHelper
{
	public static Guid GetUserId(this HttpContext context)
    {
        var parsed = Guid.TryParse(context.User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);
        if (parsed)
        {
            return userId;
        }   

        throw new CustomApiException
        {
            StatusCode = StatusCodes.Status401Unauthorized,
            Title = "Not authorized",
            Detail = "Authorization data invalid"
        };
    }
}