using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.BL.Services;
using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.Domain.DTOs;
using ProjectCollaborationPlatform.Domain.Helpers;
using ProjectCollaborationPlatform.Domain.Pagination;
using System.Security.Claims;

namespace ProjectCollaborationPlatform.WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [Authorize]
        [Authorize(Policy = "ProjectOwnerRole")]
        [HttpPost("{devId:Guid}")]
        public async Task<IActionResult> AddFeedback([FromRoute] Guid devId, [FromBody] FeedbackDTO feedbackDTO)
        {
            Guid projOwnerId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (feedbackDTO == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Title = "Bad request",
                    Detail = "Can't create a feedback"
                };
            }

            if (await _feedbackService.AddFeedback(projOwnerId, devId, feedbackDTO))
            {
                return Ok();
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
        [Authorize(Policy = "ProjectOwnerRole")]
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteFeedback([FromRoute] Guid id, CancellationToken token)
        {
            if (await _feedbackService.DeleteFeedback(id, token))
            {
                return NoContent();
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
        [Authorize(Policy = "ProjectOwnerRole")]
        [HttpPatch("{id:Guid}")]
        public async Task<IActionResult> UpdateFeedback([FromRoute] Guid id, [FromBody] FeedbackDTO feedbackDTO)
        {
            if (await _feedbackService.UpdateFeedback(id, feedbackDTO))
            {
                return NoContent();
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

        [Authorize(Policy = "AdminProjectOwnerRole")]
        [HttpGet("{devId:Guid}")]
        public async Task<IActionResult> GetAllDeveloperFeedbacks([FromQuery] PaginationFilter paginationFilter, [FromRoute] Guid devId, CancellationToken token)
        {
            var filter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
            var feedbacks = await _feedbackService.GetAllDeveloperFeedbacks(filter, devId, token);

            return Ok(feedbacks);
        }

        [Authorize]
        [Authorize(Policy = "DeveloperRole")]
        [HttpGet]
        public async Task<IActionResult> GetAllFeedbacksForCurrentDev([FromQuery] PaginationFilter paginationFilter, CancellationToken token)
        {
            Guid devId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var filter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
            var feedbacks = await _feedbackService.GetAllDeveloperFeedbacks(filter, devId, token);

            return Ok(feedbacks);
        }

        [HttpGet("feedbacks")]
        [Authorize(Policy = "AdminRole")]
        public async Task<IActionResult> GetAllFeedbacks([FromQuery] PaginationFilter paginationFilter, CancellationToken token)
        {
            var filter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
            var developers = await _feedbackService.GetAllFeedbacks(filter, token);

            return Ok(developers);
        }
    }

    
}
