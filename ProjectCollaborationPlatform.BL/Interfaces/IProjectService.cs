

using ProjectCollaborationPlatform.Domain.DTOs;
using ProjectCollaborationPlatform.Domain.Pagination;

namespace ProjectCollaborationPlatform.BL.Interfaces
{
    public  interface IProjectService
    {
        Task<ProjectDTO> GetProjectByName(string name, CancellationToken token);
        Task<ProjectDTO> GetProjectById(Guid id, CancellationToken token);
        Task<bool> AddProject(ProjectFullInfoDTO projectDTO, Guid id, CancellationToken token);
        Task<bool> UpdateProject(ProjectDTO projectDTO, Guid id);
        Task<bool> UpdateProjectDetails(Guid id, string description);
        Task<bool> DeleteProjectById(Guid id, CancellationToken token);
        Task<bool> DeleteProjectByName(string name, CancellationToken token);
        Task<bool> SaveProjectAsync();
        Task<PagedResponse<List<ProjectFullInfoDTO>>> GetAllProjects(PaginationFilter filter, CancellationToken token);
    }
}
