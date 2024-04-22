using ProjectCollaborationPlatform.Domain.DTOs;
using ProjectCollaborationPlatform.Domain.Pagination;

namespace ProjectCollaborationPlatform.BL.Interfaces
{
    public interface IDeveloperService
    {
        Task<DeveloperDTO> GetDeveloperById(Guid id, CancellationToken token);
        Task<bool> AddDeveloper(Guid id, DeveloperDTO developer);
        Task<bool> UpdateDeveloper(DeveloperDTO developerDTO);
        Task<bool> DeleteDeveloper(Guid id);
        Task<bool> SaveDeveloperAsync();
        Task<bool> IsDeveloperExists(string email);
        Task<bool> AddDeveloperForProject(Guid id, List<Guid> techId);
        Task<bool> RemoveDeveloperFromProject(Guid id, List<Guid> techId);
        Task<PagedResponse<List<PaginationDeveloperDTO>>> GetAllDevelopers(PaginationFilter filter, CancellationToken token);
    }
}
