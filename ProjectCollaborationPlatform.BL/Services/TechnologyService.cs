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
        public async Task<bool> AddTechnologyForProject(Guid id, List<string> techId)
        {
            var project = await _context.Projects.AnyAsync(i => i.Id == id);
            if (!project)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Project not found",
                    Detail = "Project with such id not found"
                };
            }

            var isTechExist = techId.All(techId => _context.Technologies.Any(x => x.Id == Guid.Parse(techId)));

            if (!isTechExist)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Technologies not found",
                    Detail = "Technologies with such ids not found"
                };
            }

            var projTechnologies = techId.Select(pt => new ProjectTechnology
            {
                ProjectID = id,
                TechnologyID = Guid.Parse(pt)
            }).ToList();

            _context.ProjectTechnologies.AddRange(projTechnologies);

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

        public async Task<bool> RemoveTechnologyFromProject(Guid id, List<string> techId)
        {
            var project = await _context.Projects
                                          .AnyAsync(i => i.Id == id);

            if (!project)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Project not found",
                    Detail = "Project with such id not found"
                };
            }
            var isTechExist = techId.All(techId =>
                _context.Technologies.Any(x => x.Id == Guid.Parse(techId)));

            if (!isTechExist)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Technologies not found",
                    Detail = "Technologies with such ids not found"
                };
            }

            var projectTechnologiesToRemove = techId.Select(techId => new ProjectTechnology
            {
                ProjectID = id,
                TechnologyID = Guid.Parse(techId)
            }).ToList();

            _context.ProjectTechnologies.RemoveRange(projectTechnologiesToRemove);

            return await SaveTechnologiesAsync();
        }

        public async Task<bool> RemoveTechnologyFromDeveloper(Guid id, List<string> techId)
        {
            var developer = await _context.Developers
                                          .AnyAsync(i => i.Id == id);

            if (!developer)
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

            var developerTechnologiesToRemove = techId.Select(techId => new DeveloperTechnology
            {
                DeveloperID = id,
                TechnologyID = Guid.Parse(techId)
            }).ToList();

            _context.DeveloperTechnologies.RemoveRange(developerTechnologiesToRemove);

            return await SaveTechnologiesAsync();
        }

        public async Task<bool> SaveTechnologiesAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }
    }
}
