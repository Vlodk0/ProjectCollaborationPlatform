

using ProjectCollaborationPlatform.Domain.DTOs;
using ProjectCollaborationPlatform.Domain.Pagination;

namespace ProjectCollaborationPlatform.BL.Interfaces
{
    public  interface IProjectService
    {
        Task<ProjectDTO> GetProjectByName(string name, CancellationToken token);
        Task<GetProjectDTO> GetProjectById(Guid id, CancellationToken token);
        Task<bool> AddProject(CreateProjectDTO projectDTO, Guid id, CancellationToken token);
        Task<bool> UpdateProject(CreateProjectDTO projectDTO, Guid id);
        Task<bool> UpdateProjectDetails(Guid id, string description);
        Task<bool> DeleteProjectById(Guid id, CancellationToken token);
        Task<bool> DeleteProjectByName(string name, CancellationToken token);
        Task<bool> SaveProjectAsync();
        Task<PagedResponse<List<ProjectFullInfoDTO>>> GetAllProjects(PaginationFilter filter, CancellationToken token);
        Task<PagedResponse<List<ProjectFullInfoDTO>>> GetAllProjectsByProjectOwnerId(Guid id, PaginationFilter filter, CancellationToken token);
        Task<PagedResponse<List<ProjectFullInfoDTO>>> GetAllProjectsWhereDevsExists(Guid id, PaginationFilter filter, CancellationToken token);
        Task<List<ProjectDTO>> GetProjectOwnerListProjects(Guid projOwnerId, CancellationToken token);
    }
}
