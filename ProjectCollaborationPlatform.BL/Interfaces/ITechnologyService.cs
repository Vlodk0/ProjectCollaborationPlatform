

using ProjectCollaborationPlatform.Domain.DTOs;

namespace ProjectCollaborationPlatform.BL.Interfaces
{
    public interface ITechnologyService
    {
        public Task<bool> AddTechnologyForProject(Guid Id, List<ProjectTechnologyIdDTO> projectTechnologyIdDTO);
        public Task<bool> RemoveTechnologyForProject(Guid Id, List<ProjectTechnologyIdDTO?> projectTechnologyIdDTO);
        public Task<bool> SaveTechnologiesAsync();
    }
}
