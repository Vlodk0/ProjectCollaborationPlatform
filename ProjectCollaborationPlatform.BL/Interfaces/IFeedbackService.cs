using ProjectCollaborationPlatform.Domain.DTOs;
using ProjectCollaborationPlatform.Domain.Pagination;

namespace ProjectCollaborationPlatform.BL.Interfaces
{
    public interface IFeedbackService
    {
        Task<bool> AddFeedback(Guid projectOwnerId, Guid devId, FeedbackDTO feedbackDTO);
        Task<bool> UpdateFeedback(Guid id, FeedbackDTO feedbackDTO);
        Task<bool> DeleteFeedback(Guid id, CancellationToken token);
        Task<PagedResponse<List<GetFeedbackDTO>>> GetAllDeveloperFeedbacks(PaginationFilter filter, Guid id, CancellationToken token);
        Task<PagedResponse<List<GetFeedbackDTO>>> GetAllFeedbacks(PaginationFilter filter, CancellationToken token);
        Task<bool> SaveFeedbackAsync();
    }
}
