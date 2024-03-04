using ProjectCollaborationPlatform.DAL.Data.Models;

namespace ProjectCollaborationPlatform.BL.Interfaces
{
    public interface IFunctionalityBlockService
    {
        Task<FunctionalityBlock> GetFunctionalityBlockByName(string name, CancellationToken token);
        Task<FunctionalityBlock> GetFunctionalityBlockById(Guid id, CancellationToken token);
        Task<bool> CreateFunctionalityBlock(FunctionalityBlock functionalityBlock);
        Task<bool> UpdateFunctionalityBlock(FunctionalityBlock functionalityBlock);
        Task<bool> DeleteFunctionalityBlock(Guid id);
        Task<bool> SaveFunctionalityBlockAsync();
    }
}
