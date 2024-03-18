using Microsoft.EntityFrameworkCore;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.DAL.Data.DataAccess;
using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.Domain.DTOs;

namespace ProjectCollaborationPlatform.BL.Services
{
    public class TeamService : ITeamService
    {
        readonly ProjectPlatformContext _context;

        public TeamService(ProjectPlatformContext context)
        {
            _context = context;
        }

        public async Task<bool> AddUsersToTeam(Guid Id, List<TeamDeveloperIdDTO> developerIdDTO)
        {
            var team = await _context.Teams
                                     .Include(u => u.TeamDevelopers)
                                     .FirstOrDefaultAsync(i => i.Id == Id);
            if (team == null)
            {
                return false;
            }

            var developerIds = developerIdDTO.Select(dto => dto.developerId).ToList();

            var addedDevelopers = await _context.Developers
                                                .Where(d => developerIds.Contains(d.Id))
                                                .ToListAsync();

            var teamDevelopersToAdd = addedDevelopers.Select(developer => new TeamDeveloper
            {
                Developer = developer,
                Team = team
            }).ToList();

            team.TeamDevelopers.AddRange(teamDevelopersToAdd);


            return await SaveTeamAsync();
        }

        public async Task<bool> UpdateTeam(Guid Id, List<TeamDeveloperIdDTO> teamDeveloperIdDTO)
        {
            var team = await _context.Teams
                                     .Include(td => td.TeamDevelopers)
                                     .FirstOrDefaultAsync(i => i.Id == Id);

            if (team == null)
            {
                return false;
            }

            var developersToRemove = teamDeveloperIdDTO.Select(dto => dto.developerId).ToList();

            if (developersToRemove == null)
            {
                return false;
            }

            team.TeamDevelopers.RemoveAll(td => developersToRemove.Contains(td.DeveloperID));


            return await SaveTeamAsync();
        }

        public async Task<List<Team>> GetTeamsWithDevelopers()
        {
            var teamWithDevelopers = await _context.Teams
                                                   .Include(td => td.TeamDevelopers)
                                                   .ThenInclude(d => d.Developer)
                                                   .ToListAsync();
            return teamWithDevelopers;
        }

        public async Task<bool> SaveTeamAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }
    }
}
