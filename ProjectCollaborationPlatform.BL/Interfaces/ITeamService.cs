

using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.Domain.DTOs;

namespace ProjectCollaborationPlatform.BL.Interfaces
{
    public interface ITeamService
    {
        public Task<bool> AddUsersToTeam(Guid Id, List<TeamDeveloperIdDTO> teamDeveloperIdDTO);
        public Task<List<Team>> GetTeamsWithDevelopers();
        public Task<bool> UpdateTeam(Guid Id, List<TeamDeveloperIdDTO> teamDeveloperIdDTO);
        Task<bool> CreateTeam(TeamDTO teamDTO);
        Task<bool> SaveTeamAsync();
        public Task<TeamDTO> GetTeamById(Guid Id, CancellationToken token);
    }
}
