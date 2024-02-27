using ProjectCollaborationPlatform.DAL.Data.Models;

namespace ProjectCollaborationPlatform.BL.Interfaces
{
    public interface IFunctionalityBlockService
    {
        Task<FunctionalityBlock> GetFunctionalityBlockByName(string name);
        Task<FunctionalityBlock> GetFunctionalityBlockById(Guid id);
        Task<bool> CreateFunctionalityBlock(FunctionalityBlock functionalityBlock);
        Task<bool> UpdateFunctionalityBlock(FunctionalityBlock functionalityBlock);
        Task<bool> DeleteFunctionalityBlock(Guid id);
        Task<bool> SaveFunctionalityBlockAsync();
    }
}
