using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.DAL.Data.DataAccess;
using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.DAL.Models;
using ProjectCollaborationPlatform.Domain.DTOs;
using ProjectCollaborationPlatform.Domain.Helpers;
using ProjectCollaborationPlatform.Domain.Pagination;
using System.Xml.Linq;

namespace ProjectCollaborationPlatform.BL.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly ProjectPlatformContext _context;

        public FeedbackService(ProjectPlatformContext context)
        {
            _context = context;
        }

        public Task<bool> AddFeedback(Guid projectOwnerId, Guid devId, FeedbackDTO feedbackDTO)
        {
            var feedback = new Feedback()
            {
                Content = feedbackDTO.Content,
                ProjectOwnerID = projectOwnerId,
                DeveloperId = devId,
            };

            _context.Feedbacks.Add(feedback);

            return SaveFeedbackAsync();
        }

        public async Task<bool> DeleteFeedback(Guid id, CancellationToken token)
        {
            var feedback = await _context.Feedbacks.Where(i => i.Id == id).FirstOrDefaultAsync(token);

            if (feedback == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Not found",
                    Detail = "Feedback with such id not found"
                };
            }

            _context.Feedbacks.Remove(feedback);

            return await SaveFeedbackAsync();
        }

        public async Task<PagedResponse<List<GetFeedbackDTO>>> GetAllDeveloperFeedbacks(PaginationFilter filter, Guid id, CancellationToken token)
        {

            IQueryable<Feedback> query = _context.Feedbacks
                .Where(f => f.DeveloperId == id);


            var totalRecords = await query.CountAsync(token);
            var totalPages = (int)Math.Ceiling(totalRecords / (double)filter.PageSize);

            query = query
                .Skip(filter.PageNumber)
                .Take(filter.PageSize);

            if (id == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Not found",
                    Detail = "Developer with such id not found"
                };
            }

            var funcBlocks = await query
                .Select(f => new GetFeedbackDTO
                {
                    Id = f.Id,
                    Content = f.Content,
                })
                .ToListAsync(token);

            return new PagedResponse<List<GetFeedbackDTO>>(funcBlocks, filter.PageNumber, filter.PageSize, totalRecords, totalPages);
        }

        public async Task<bool> SaveFeedbackAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UpdateFeedback(Guid id, FeedbackDTO feedbackDTO)
        {
            var feedback = await _context.Feedbacks.Where(i => i.Id == id).FirstOrDefaultAsync();

            if (feedback == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Not found",
                    Detail = "Feedback with such id not found"
                };
            }

            feedback.Content = feedbackDTO.Content;
            _context.Feedbacks.Update(feedback);
            return await SaveFeedbackAsync();
        }
    }
}
