using ProjectCollaborationPlatform.Domain.DTOs;

namespace ProjectCollaborationPlatform.BL.Interfaces
{
    public interface IFunctionalityBlockService
    {
        Task<FunctionalityBlockDTO> GetFunctionalityBlockByName(string name, CancellationToken token);
        Task<FunctionalityBlockDTO> GetFunctionalityBlockById(Guid id, CancellationToken token);
        Task<bool> CreateFunctionalityBlock(FunctionalityBlockDTO functionalityBlockDto, Guid boardId);
        Task<bool> UpdateFunctionalityBlock(string name);
        Task<bool> DeleteFunctionalityBlock(Guid id);
        Task<bool> SaveFunctionalityBlockAsync();
    }
}
