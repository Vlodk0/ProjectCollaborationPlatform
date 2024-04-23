using Microsoft.EntityFrameworkCore;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.DAL.Data.DataAccess;
using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.Domain.DTOs;
using ProjectCollaborationPlatform.Domain.Pagination;

namespace ProjectCollaborationPlatform.BL.Services
{
    public class ProjectOwnerService : IProjectOwnerService
    {
        readonly ProjectPlatformContext _context;

        public ProjectOwnerService(ProjectPlatformContext context)
        {
            _context = context;
        }
        public async Task<bool> AddProjectOwner(Guid id, ProjectOwnerDTO projectOwnerDTO)
        {
            var projectOwner = new ProjectOwner()
            {
                Id = id,
                FirstName = projectOwnerDTO.FirstName,
                LastName = projectOwnerDTO.LastName,
                Email = projectOwnerDTO.Email,
                IsDeleted = false,
            };
            _context.Set<ProjectOwner>().Add(projectOwner);
            return await SaveProjectOwnerAsync();
        }

        public async Task<bool> DeleteProjectOwner(Guid id)
        {
            var projectOwner = await _context.ProjectOwners.Where(i => i.Id == id).FirstOrDefaultAsync();
            if (projectOwner == null)
            {
                return false;
            }
            var deletedProjectOwner = new ProjectOwner()
            {
                FirstName = projectOwner.FirstName,
                LastName = projectOwner.LastName,
                Email = projectOwner.Email,
                IsDeleted = true,
            };

            _context.ProjectOwners.Remove(projectOwner);
            _context.ProjectOwners.Update(deletedProjectOwner);

            return await SaveProjectOwnerAsync();
        }

        public async Task<PagedResponse<List<ProjectOwnerPaginationDTO>>> GetAllProjectOwners(PaginationFilter filter, CancellationToken token)
        {
            IQueryable<ProjectOwner> query = _context.ProjectOwners
                .Where(i => i.IsDeleted == false);

            var totalRecords = await query.CountAsync(token);
            var totalPages = (int)Math.Ceiling(totalRecords / (double)filter.PageSize);

            query = query
                .Skip(filter.PageNumber)
                .Take(filter.PageSize);

            var result = await query
                .Select(t => new ProjectOwnerPaginationDTO
                {
                    Id = t.Id,
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                }).ToListAsync(token);

            return new PagedResponse<List<ProjectOwnerPaginationDTO>>(result, filter.PageNumber, filter.PageSize, totalRecords, totalPages);
        }

        public async Task<ProjectOwnerDTO> GetProjectOwnerByEmail(string email, CancellationToken token)
        {
            var projectOwner = await _context.ProjectOwners.Where(e => e.Email == email).FirstOrDefaultAsync(token);

            return new ProjectOwnerDTO()
            {
                Id = projectOwner.Id,
                Email = email,
                FirstName = projectOwner.FirstName,
                LastName = projectOwner.LastName,
            };
        }

        public async Task<ProjectOwnerDTO> GetProjectOwnerById(Guid id, CancellationToken token)
        {
            var projectOwner = await _context.ProjectOwners.Where(i => i.Id == id).FirstOrDefaultAsync(token);

            if (projectOwner == null)
            {
                return null;
            }

            return new ProjectOwnerDTO()
            {
                Id = id,
                Email = projectOwner.Email,
                FirstName = projectOwner.FirstName,
                LastName = projectOwner.LastName,
                RoleName = "ProjectOwner",
                IsDeleted = projectOwner.IsDeleted,
            };
        }

        public Task<bool> IsProjectOwnerExists(string email)
            => _context.ProjectOwners.AnyAsync(u => u.Email == email);

        public async Task<bool> SaveProjectOwnerAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UpdateProjectOwner(Guid id, UpdateUserDTO userDTO)
        {
            var projectOwner = await _context.ProjectOwners.Where(e => e.Id == id).FirstOrDefaultAsync();

            projectOwner.FirstName = userDTO.FirstName;
            projectOwner.LastName = userDTO.LastName;
            _context.ProjectOwners.Update(projectOwner);
            return await SaveProjectOwnerAsync();
        }
    }
}
