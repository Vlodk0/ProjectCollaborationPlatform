﻿using Microsoft.AspNetCore.Http;
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

            var title = await _context.Projects.Where(p => p.Title == projectDTO.Title).FirstOrDefaultAsync(token);
            return await AddProjectDetails(title.Id, projectDTO.Description);
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
                Title = project.Title,
                Payment = project.Payment,
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
                Title = project.Title,
                Payment = project.Payment
            };
        }

        public async Task<bool> SaveProjectAsync()
        {
            try
            {
                var saved = await _context.SaveChangesAsync();
                return saved > 0 ? true : false;
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
