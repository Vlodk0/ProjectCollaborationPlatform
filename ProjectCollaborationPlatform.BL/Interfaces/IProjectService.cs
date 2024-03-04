

using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.Domain.DTOs;

namespace ProjectCollaborationPlatform.BL.Interfaces
{
    public  interface IProjectService
    {
        Task<Project> GetProjectByName(string name, CancellationToken token);
        Task<Project> GetProjectById(Guid id, CancellationToken token);
        Task<bool> AddProject(ProjectDTO projectDTO);
        Task<bool> UpdateProject(ProjectDTO projectDTO);
        Task<bool> DeleteProjectById(Guid id);
        Task<bool> DeleteProjectByName(string name);
        Task<bool> SaveProjectAsync();
        Task<List<Project>> GetAllProjects(CancellationToken token);
    }
}
