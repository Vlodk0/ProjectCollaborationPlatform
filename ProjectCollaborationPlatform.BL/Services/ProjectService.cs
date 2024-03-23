using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.DAL.Data.DataAccess;
using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.Domain.DTOs;
using ProjectCollaborationPlatform.Domain.Helpers;
using ProjectCollaborationPlatform.Domain.Pagination;
using System.Xml.Linq;

namespace ProjectCollaborationPlatform.BL.Services
{
    public class ProjectService : IProjectService
    {
        readonly ProjectPlatformContext _context;

        public ProjectService(ProjectPlatformContext context)
        {
            _context = context;
        }

        public async Task<bool> AddProject(ProjectDTO projectDTO)
        {
            var project = new Project()
            {
                Title = projectDTO.Title,
                Payment = projectDTO.Payment,
            };
            _context.Set<Project>().Add(project);
            return await SaveProjectAsync();
        }

        public async Task<bool> DeleteProjectById(Guid id)
        {
            var entity = await _context.Set<Project>().FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            _context.Set<Project>().Remove(entity);

            return await SaveProjectAsync();
        }

        public async Task<bool> DeleteProjectByName(string name)
        {
            var entity = await _context.Set<Project>().FindAsync(name);
            if (entity == null)
            {
                return false;
            }

            _context.Set<Project>().Remove(entity);

            return await SaveProjectAsync();
        }

        public async Task<PagedResponse<List<ProjectFullInfoDTO>>> GetAllProjects(PaginationFilter filter, CancellationToken token)
        {
            IQueryable<Project> query = _context.Projects;

            query = filter.SortColumn switch
            {
                "Payment" when filter.SortDirection == "asc" =>
                    query.OrderBy(p => p.Payment),
                "Payment" => query.OrderByDescending(p => p.Payment),
                _ => query
            };

            query = query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize);

            var result = await query
                .Include(pd => pd.ProjectDetail)
                .Select(p => new ProjectFullInfoDTO()
                {
                    Payment = p.Payment,
                    Title = p.Title,
                    Description = p.ProjectDetail.Description
                }).ToListAsync(token);

            return new PagedResponse<List<ProjectFullInfoDTO>>(result, filter.PageNumber, filter.PageSize);
        }

        public async Task<ProjectDTO> GetProjectById(Guid id, CancellationToken token)
        {
            var project = await _context.Projects.Where(i => i.Id == id).FirstOrDefaultAsync(token);

            if (project == null)
            {
                return null;
            }

            return new ProjectDTO()
            {
                Id = project.Id,
                Title = project.Title,
            };
        }

        public async Task<ProjectDTO> GetProjectByName(string name, CancellationToken token)
        {
            var project = await _context.Projects.Where(t => t.Title == name).FirstOrDefaultAsync(token);

            if (project == null)
            {
                return null;
            }

            return new ProjectDTO()
            {
                Id = project.Id,
                Title = project.Title,
            };
        }

        public async Task<bool> SaveProjectAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateProject(ProjectDTO projectDTO)
        {
            var project = await _context.Projects.Where(e => e.Title == projectDTO.Title).FirstOrDefaultAsync();
            project = new Project()
            {
                Title = projectDTO.Title,
                Payment = projectDTO.Payment,
            };
            _context.Projects.Update(project);
            return await SaveProjectAsync();
        }
    }
}
