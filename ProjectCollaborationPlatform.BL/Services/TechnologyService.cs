using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.DAL.Data.DataAccess;
using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.Domain.DTOs;
using ProjectCollaborationPlatform.Domain.Helpers;
using System.Threading;

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
            bool dev = await _context.Developers.AnyAsync(i => i.Id == id);//why not var devExists = ...?
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
                TechnologyID = Guid.Parse(techId)//guid.parse is done twice. it was better to store parsed values in variable
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
                _context.Technologies.Any(x => x.Id == Guid.Parse(techId)));//a lot of sync db calls. you could make it in parallel
                                                                            //var tasks = techId.Select(techId =>
                                                                            // _context.Technologies.AnyAsync(x => x.Id == Guid.Parse(techId))) 
                                                                            //var results = await Task.WhenAll(tasks);
                                                                            //var allTechIdsExist = results.All(x => x);
                                                                            //also this code(lines 147-169) is common for most of your methods. can be a separate method

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

        public async Task<List<TechnologyDTO>> GetAllTechnologies(CancellationToken token)
        {
            var techs = await _context.Technologies
                .Select(t => new TechnologyDTO
                {
                    Id = t.Id,
                    Framework = t.Framework,
                    Technology = t.Language,
                }).ToListAsync(token);

            return techs;
        }

        public async Task<List<TechnologyDTO>> GetAllTechnologiesByProjectId(Guid projId, CancellationToken token)
        {
            var technologies = await _context.Projects
                            .Where(p => p.Id == projId)//it must be .FirstOrDefault instead of Where. then you can map your technologies
                            .SelectMany(p => p.ProjectTechnologies.Select(pt => pt.Technology))
                            .Select(t => new TechnologyDTO
                            {
                                Id = t.Id,
                                Framework = t.Framework,
                                Technology = t.Language
                            })
                            .ToListAsync(token);

            return technologies;
        }

        public async Task<List<TechnologyDTO>> GetAllDeveloperTechnologies(Guid devId, CancellationToken token)
        {
            var technologies = await _context.Developers
                            .Where(p => p.Id == devId)
                            .SelectMany(p => p.DeveloperTechnologies.Select(pt => pt.Technology))
                            .Select(t => new TechnologyDTO
                            {
                                Id = t.Id,
                                Framework = t.Framework,
                                Technology = t.Language
                            })
                            .ToListAsync(token);

            return technologies;
        }

        public async Task<List<CountTechnologyOnProjectsDTO>> GetTechnologyStatisticByProjects(CancellationToken token)
        {
            var projectsWithTechnologies = await _context.Projects
                .Include(p => p.ProjectTechnologies)
                .ThenInclude(pt => pt.Technology)
                .ToListAsync(token);

            var technologyStats = projectsWithTechnologies
                .SelectMany(p => p.ProjectTechnologies.Select(pt => new { ProjectId = p.Id, Technology = pt.Technology }))
                .GroupBy(pt => pt.Technology.Language)
                .Select(group => new CountTechnologyOnProjectsDTO
                {
                    Technology = group.First().Technology.Language,
                    Count = group.Count()
                })
                .ToList();

            return technologyStats;


        }
    }
}
