using ProjectCollaborationPlatform.Domain.DTOs;
using ProjectCollaborationPlatform.Domain.Enums;

namespace ProjectCollaborationPlatform.BL.Interfaces
{
    public interface IFunctionalityBlockService
    {
        Task<FunctionalityBlockDTO> GetFunctionalityBlockById(Guid id, CancellationToken token);
        Task<List<FunctionalityBlockDTO>> GetFunctionalityBlocksByBoardId(Guid boardId, CancellationToken token);
        Task<bool> UpdateFunctionalityBlockStatus(Guid id, StatusEnum status);
        Task<bool> CreateFunctionalityBlock(FunctionalityBlockDTO functionalityBlockDto, Guid boardId);
        Task<bool> UpdateFunctionalityBlock(Guid id, string task);
        Task<bool> DeleteFunctionalityBlock(Guid id);
        Task<bool> SaveFunctionalityBlockAsync();
    }
}
