

using ProjectCollaborationPlatform.DAL.Data.Models;

namespace ProjectCollaborationPlatform.BL.Interfaces
{
    public  interface IProjectService : IBaseService<Project>
    {
        Task<List<Project>> GetAllProjects();
    }
}
