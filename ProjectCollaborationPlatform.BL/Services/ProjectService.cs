using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.DAL.Data.DataAccess;
using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.Domain.DTOs;
using ProjectCollaborationPlatform.Domain.Helpers;
using ProjectCollaborationPlatform.Domain.Pagination;
using System.Linq;
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

        public async Task<bool> AddProject(ProjectFullInfoDTO projectDTO, Guid id, CancellationToken token)
        {
            var project = new Project()
            {
                Title = projectDTO.Title,
                Payment = projectDTO.Payment,
                ProjectOwnerID = id,
            };

            _context.Projects.Add(project);
            if (!await SaveProjectAsync())
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Title = "Server Error",
                    Detail = "Error occured while creating project"
                };
            }

            var prj = await _context.Projects.Where(p => p.Title == projectDTO.Title).FirstOrDefaultAsync(token);

            await AddProjectDetails(prj.Id, projectDTO.Description);

            return await AddBoardOnTheProject(prj.Id, projectDTO.BoardName);
        }

        private async Task<bool> AddProjectDetails(Guid id, string description)
        {
            var projectDetails = new ProjectDetail()
            {
                ProjectID = id,
                Description = description,
            };
            _context.ProjectDetails.Add(projectDetails);
            return await SaveProjectAsync();
        }

        private async Task<bool> AddBoardOnTheProject(Guid id, string name)
        {
            var board = new Board()
            {
                Name = name,
                ProjectID = id,
            };
            _context.Boards.Add(board);
            return await SaveProjectAsync();
        }

        public async Task<bool> DeleteProjectById(Guid id, CancellationToken token)
        {
            var entity = await _context.Projects.Where(p => p.Id == id).FirstOrDefaultAsync(token);
            if (entity == null)
            {
                return false;
            }

            _context.Projects.Remove(entity);

            return await SaveProjectAsync();
        }

        public async Task<bool> DeleteProjectByName(string name, CancellationToken token)
        {
            var entity = await _context.Projects.Where(p => p.Title == name).FirstOrDefaultAsync(token);
            if (entity == null)
            {
                return false;
            }

            _context.Projects.Remove(entity);

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
                .Skip(filter.PageNumber)
                .Take(filter.PageSize);

            var totalRecords = await _context.Projects.CountAsync();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)filter.PageSize);

            var result = await query
                .Include(pd => pd.ProjectDetail)
                .Include(pt => pt.ProjectTechnologies)
                .Select(p => new ProjectFullInfoDTO()
                {
                    Id = p.Id,
                    Payment = p.Payment,
                    Title = p.Title,
                    Description = p.ProjectDetail.Description,
                    Technologies = p.ProjectTechnologies.Select(i => new DeveloperTechnologyDTO
                    {
                        Technology = i.Technology.Language,
                        Framework = i.Technology.Framework
                    }).ToList(),
                }).ToListAsync(token);

            return new PagedResponse<List<ProjectFullInfoDTO>>(result, filter.PageNumber, filter.PageSize, totalRecords, totalPages);
        }

        public async Task<GetProjectDTO> GetProjectById(Guid id, CancellationToken token)
        {
       
            return await _context.Projects
                .Include(pd => pd.ProjectDetail)
                .Include(pt => pt.ProjectTechnologies)
                .Where(i => i.Id == id)
                .Select(t => new GetProjectDTO()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Payment = t.Payment,
                    Description = t.ProjectDetail.Description,
                    Technologies = t.ProjectTechnologies.Select(i => new DeveloperTechnologyDTO
                    {
                        Technology = i.Technology.Language,
                        Framework = i.Technology.Framework
                    }).ToList()
                }).FirstAsync(token);

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
                Title = project.Title,
                Payment = project.Payment
            };
        }

        public async Task<bool> SaveProjectAsync()
        {
            try
            {
                var saved = await _context.SaveChangesAsync();
                return saved > 0;
            }
            catch (Exception ex)
            {
                throw new CustomApiException
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Title = "Error",
                    Detail = ex.Message
                };
            }
        }

        public async Task<bool> UpdateProject(ProjectDTO projectDTO, Guid id)
        {
            var project = await _context.Projects.Where(e => e.Id == id).FirstOrDefaultAsync();

            project.Title = projectDTO.Title;
            project.Payment = projectDTO.Payment;
            _context.Projects.Update(project);
            return await SaveProjectAsync();
        }

        public async Task<bool> UpdateProjectDetails(Guid id, string description)
        {
            var projectDetail = await _context.ProjectDetails.Where(e => e.ProjectID == id).FirstOrDefaultAsync();

            projectDetail.Description = description;
            _context.ProjectDetails.Update(projectDetail);
            return await SaveProjectAsync();
        }
    }
}
