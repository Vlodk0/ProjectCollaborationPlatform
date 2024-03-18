

using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.Domain.DTOs;
using ProjectCollaborationPlatform.Domain.Pagination;

namespace ProjectCollaborationPlatform.BL.Interfaces
{
    public  interface IProjectService
    {
        Task<ProjectDTO> GetProjectByName(string name, CancellationToken token);
        Task<ProjectDTO> GetProjectById(Guid id, CancellationToken token);
        Task<bool> AddProject(ProjectDTO projectDTO);
        Task<bool> UpdateProject(ProjectDTO projectDTO);
        Task<bool> DeleteProjectById(Guid id);
        Task<bool> DeleteProjectByName(string name);
        Task<bool> SaveProjectAsync();
        Task<PagedResponse<List<ProjectDTO>>> GetAllProjects(PaginationFilter filter, CancellationToken token);
    }
}
