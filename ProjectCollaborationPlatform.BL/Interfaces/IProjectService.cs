

using ProjectCollaborationPlatform.DAL.Data.Models;

namespace ProjectCollaborationPlatform.BL.Interfaces
{
    public  interface IProjectService
    {
        Task<Project> GetProjectByName(string name);
        Task<Project> GetProjectById(Guid id);
        Task<bool> AddProject(Project project);
        Task<bool> UpdateProject(Project project);
        Task<bool> DeleteProjectById(Guid id);
        Task<bool> DeleteProjectByName(string name);
        Task<bool> SaveProjectAsync();
        Task<List<Project>> GetAllProjects();
    }
}
