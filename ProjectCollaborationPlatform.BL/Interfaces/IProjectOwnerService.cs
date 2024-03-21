using ProjectCollaborationPlatform.Domain.DTOs;

namespace ProjectCollaborationPlatform.BL.Interfaces
{
    public interface IProjectOwnerService
    {
        Task<ProjectOwnerDTO> GetProjectOwnerById(Guid id, CancellationToken token);
        Task<ProjectOwnerDTO> GetProjectOwnerByEmail(string email, CancellationToken token);
        Task<bool> AddProjectOwner(Guid id, ProjectOwnerDTO projectOwnerDTO);
        Task<bool> UpdateProjectOwner(ProjectOwnerDTO projectOwnerDTO);
        Task<bool> DeleteProjectOwner(Guid id);
        Task<bool> SaveProjectOwnerAsync();
        Task<bool> IsProjectOwnerExists(string email);
    }
}
