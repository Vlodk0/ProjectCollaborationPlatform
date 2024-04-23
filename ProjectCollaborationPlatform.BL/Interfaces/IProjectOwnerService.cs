using ProjectCollaborationPlatform.Domain.DTOs;
using ProjectCollaborationPlatform.Domain.Pagination;

namespace ProjectCollaborationPlatform.BL.Interfaces
{
    public interface IProjectOwnerService
    {
        Task<ProjectOwnerDTO> GetProjectOwnerById(Guid id, CancellationToken token);
        Task<ProjectOwnerDTO> GetProjectOwnerByEmail(string email, CancellationToken token);
        Task<bool> AddProjectOwner(Guid id, ProjectOwnerDTO projectOwnerDTO);
        Task<bool> UpdateProjectOwner(Guid id, UpdateUserDTO userDTO);
        Task<bool> DeleteProjectOwner(Guid id);
        Task<bool> SaveProjectOwnerAsync();
        Task<bool> IsProjectOwnerExists(string email);
        Task<PagedResponse<List<ProjectOwnerPaginationDTO>>> GetAllProjectOwners(PaginationFilter filter, CancellationToken token);

    }
}
