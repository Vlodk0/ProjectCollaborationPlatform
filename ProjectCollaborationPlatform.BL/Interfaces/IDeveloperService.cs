

using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.Domain.DTOs;

namespace ProjectCollaborationPlatform.BL.Interfaces
{
    public interface IDeveloperService
    {
        Task<DeveloperDTO> GetDeveloperById(Guid id, CancellationToken token);
        Task<DeveloperDTO> GetDeveloperByEmail(string email, CancellationToken token);
        Task<bool> AddDeveloper(Guid id, DeveloperDTO developer);
        Task<bool> UpdateDeveloper(DeveloperDTO developerDTO);
        Task<bool> DeleteDeveloper(Guid id);
        Task<bool> SaveDeveloperAsync();
        Task<bool> IsDeveloperExists(string email);

    }
}
