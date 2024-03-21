using Microsoft.EntityFrameworkCore;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.DAL.Data.DataAccess;
using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.Domain.DTOs;

namespace ProjectCollaborationPlatform.BL.Services
{
    public class TechnologyService : ITechnologyService
    {
        private readonly ProjectPlatformContext _context;

        public TechnologyService(ProjectPlatformContext context)
        {
            _context = context;
        }
        public async Task<bool> AddTechnologyForProject(Guid Id, List<ProjectTechnologyIdDTO> projectTechnologyIdDTO)
        {
            var project = await _context.Projects
                                        .Include(pt => pt.ProjectTechnologies)
                                        .FirstOrDefaultAsync(i => i.Id == Id);
            if (project == null)
            {
                return false;
            }

            var technologiesIds = projectTechnologyIdDTO.Select(dto => dto.TechnologyId).ToList();

            var addedTechnologies = await _context.Technologies
                                                  .Where(t => technologiesIds.Contains(t.Id))
                                                  .ToListAsync();

            var projectTechnologiesToAdd = addedTechnologies.Select(technology => new ProjectTechnology
            {
                Technology = technology,
                Project = project
            });

            return await SaveTechnologiesAsync();
        }

        public async Task<bool> RemoveTechnologyForProject(Guid Id, List<ProjectTechnologyIdDTO> projectTechnologyIdDTO)
        {
            var project = await _context.Projects
                                        .Include(pt => pt.ProjectTechnologies)
                                        .FirstOrDefaultAsync(i => i.Id == Id);

            if (project == null)
            {
                return false;
            }

            var technologiesToRemove = projectTechnologyIdDTO.Select(dto => dto.TechnologyId).ToList();

            if ( technologiesToRemove == null)
            {
                return false;
            }

            project.ProjectTechnologies.RemoveAll(pt => technologiesToRemove.Contains(pt.TechnologyID));

            return await SaveTechnologiesAsync();
        }

        public async Task<bool> SaveTechnologiesAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }
    }
}
