

using ProjectCollaborationPlatform.Domain.DTOs;

namespace ProjectCollaborationPlatform.BL.Interfaces
{
    public interface ITechnologyService
    {
        public Task<bool> AddTechnologyForProject(Guid Id, List<string> techId);
        public Task<bool> AddTechnologyForDeveloper(Guid Id, List<string> techId);
        public Task<bool> RemoveTechnologyFromProject(Guid Id, List<string> techId);
        public Task<bool> RemoveTechnologyFromDeveloper(Guid Id, List<string> techId);
        public Task<bool> SaveTechnologiesAsync();
    }
}
