using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.DAL.Data.DataAccess;
using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.Domain.DTOs;
using ProjectCollaborationPlatform.Domain.Helpers;

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

        public async Task<bool> AddTechnologyForDeveloper(Guid id, List<string> techId)
        {
            bool dev = await _context.Developers.AnyAsync(i => i.Id == id);
            if (!dev)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Dev not found",
                    Detail = "Dev with such id not found"
                };
            }

            bool allTechIdsExist = techId.All(techId =>
                _context.Technologies.Any(x => x.Id == Guid.Parse(techId)));

            if (!allTechIdsExist)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Technologies not found",
                    Detail = "Technologies with such ids not found"
                };
            }

            var developerTechnologies = techId.Select(techId => new DeveloperTechnology
            {
                DeveloperID = id,
                TechnologyID = Guid.Parse(techId)
            }).ToList();

            _context.DeveloperTechnologies.AddRange(developerTechnologies);

            return await SaveTechnologiesAsync();


        }

        public async Task<bool> RemoveTechnologyFromProject(Guid Id, List<ProjectTechnologyIdDTO> projectTechnologyIdDTO)
        {
            var project = await _context.Projects
                                        .Include(pt => pt.ProjectTechnologies)
                                        .FirstOrDefaultAsync(i => i.Id == Id);

            if (project == null)
            {
                return false;
            }

            var technologiesToRemove = projectTechnologyIdDTO.Select(dto => dto.TechnologyId).ToList();

            if (technologiesToRemove == null)
            {
                return false;
            }

            project.ProjectTechnologies.RemoveAll(pt => technologiesToRemove.Contains(pt.TechnologyID));

            return await SaveTechnologiesAsync();
        }

        public async Task<bool> RemoveTechnologyFromDeveloper(Guid Id, List<DeveloperTechnologyIdDTO> developerTechnologyIdDTO)
        {
            var developer = await _context.Developers
                                          .Include(dt => dt.DeveloperTechnologies)
                                          .FirstOrDefaultAsync(i => i.Id == Id);

            if (developer == null)
            {
                return false;
            }

            var tecnologiesToRemove = developerTechnologyIdDTO.Select(dto => dto.TechnologyId).ToList();

            if (tecnologiesToRemove == null)
            {
                return false;
            }

            developer.DeveloperTechnologies.RemoveAll(dt => tecnologiesToRemove.Contains(dt.TechnologyID));

            return await SaveTechnologiesAsync();
        }

        public async Task<bool> SaveTechnologiesAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }
    }
}
