using Microsoft.EntityFrameworkCore;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.DAL.Data.DataAccess;
using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.Domain.DTOs;


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
            var projectOwner = await _context.Set<ProjectOwner>().FindAsync(id);
            if (projectOwner == null)
            {
                return false;
            }
            var deletedProjectOwner = new ProjectOwner()
            {
                Id = projectOwner.Id,
                FirstName = projectOwner.FirstName,
                LastName = projectOwner.LastName,
                Email = projectOwner.Email,
                IsDeleted = true,
            };

            _context.Set<ProjectOwner>().Update(deletedProjectOwner);

            return await SaveProjectOwnerAsync();
        }

        public async Task<ProjectOwnerDTO> GetProjectOwnerByEmail(string email, CancellationToken token)
        {
            var projectOwner = await _context.ProjectOwners.Where(e => e.Email == email).FirstOrDefaultAsync(token);

            return new ProjectOwnerDTO()
            {
                Id = projectOwner.Id,
                Email = projectOwner.Email,
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
                Id = projectOwner.Id,
                Email = projectOwner.Email,
                FirstName = projectOwner.FirstName,
                LastName = projectOwner.LastName,
            };
        }

        public Task<bool> IsProjectOwnerExists(string email)
            => _context.ProjectOwners.AnyAsync(u => u.Email == email);

        public async Task<bool> SaveProjectOwnerAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UpdateProjectOwner(ProjectOwnerDTO projectOwnerDTO)
        {
            var projectOwner = await _context.ProjectOwners.Where(e => e.Email == projectOwnerDTO.Email).FirstOrDefaultAsync();
            projectOwner = new ProjectOwner()
            {
                FirstName = projectOwnerDTO.FirstName,
                LastName = projectOwnerDTO.LastName,
                Id = projectOwnerDTO.Id,
                IsDeleted = false

            };
            _context.ProjectOwners.Update(projectOwner);
            return await SaveProjectOwnerAsync();
        }
    }
}
