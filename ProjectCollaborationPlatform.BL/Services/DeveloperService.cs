using ProjectCollaborationPlatform.DAL.Data.DataAccess;
using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.BL.Interfaces;
using Microsoft.EntityFrameworkCore;
using ProjectCollaborationPlatform.Domain.DTOs;
using ProjectCollaborationPlatform.Domain.Pagination;
using Microsoft.AspNetCore.Http;
using ProjectCollaborationPlatform.Domain.Helpers;

namespace ProjectCollaborationPlatform.BL.Services
{
    public class DeveloperService : IDeveloperService
    {
        readonly ProjectPlatformContext _context;

        public DeveloperService(ProjectPlatformContext context)
        {
            _context = context;
        }

        public async Task<bool> AddDeveloper(Guid id, DeveloperDTO developerDTO)
        {
            var dev = new Developer()
            {
                Id = id,
                FirstName = developerDTO.FirstName,
                LastName = developerDTO.LastName,
                Email = developerDTO.Email,
                IsDeleted = false,
            };
            await _context.Developers.AddAsync(dev);
            return await SaveDeveloperAsync();
        }

        public async Task<bool> DeleteDeveloper(Guid id)
        {
            var dev = await _context.Developers.Where(i => i.Id == id).FirstOrDefaultAsync();
            if (dev == null)
            {
                return false;
            }
            var deletedDev = new Developer()
            {
                FirstName = dev.FirstName,
                LastName = dev.LastName,
                Email = dev.Email,
                IsDeleted = true,
            };

            _context.Developers.Update(deletedDev);

            return await SaveDeveloperAsync();
        }

        //public async Task<DeveloperDTO> GetDeveloperByEmail(string email, CancellationToken token)
        //{
        //    var dev = await _context.Developers.Where(e => e.Email == email).FirstOrDefaultAsync(token);

        //    return new DeveloperDTO()
        //    {
        //        Id = dev.Id,
        //        Email = dev.Email,
        //        FirstName = dev.FirstName,
        //        LastName = dev.LastName,
        //    };
        //}

        public async Task<DeveloperDTO> GetDeveloperById(Guid id, CancellationToken token)
        {
            var dev = await _context.Developers.Where(i => i.Id == id).FirstOrDefaultAsync(token);

            if (dev == null)
            {
                return null;
            }

            return new DeveloperDTO()
            {
                Id = dev.Id,
                Email = dev.Email,
                FirstName = dev.FirstName,
                LastName = dev.LastName,
                RoleName = "Dev",
                IsDeleted = dev.IsDeleted
            };

        }

        public async Task<PagedResponse<List<PaginationDeveloperDTO>>> GetAllDevelopers(PaginationFilter filter, CancellationToken token)
        {
            IQueryable<Developer> query = _context.Developers;

            var totalRecords = await query.CountAsync(token);
            var totalPages = (int)Math.Ceiling(totalRecords / (double)filter.PageSize);

            query = query
                .Skip(filter.PageNumber)
                .Take(filter.PageSize);

            var result = await query
                .Include(td => td.DeveloperTechnologies)
                .ThenInclude(t => t.Technology)
                .Select(d => new PaginationDeveloperDTO()
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Technologies = d.DeveloperTechnologies.Select(i => new DeveloperTechnologyDTO()
                    {
                        Technology = i.Technology.Language,
                        Framework = i.Technology.Framework
                    }).ToList()
                }).ToListAsync(token);

            return new PagedResponse<List<PaginationDeveloperDTO>>(result, filter.PageNumber, filter.PageSize, totalRecords, totalPages);
        }

        public Task<bool> IsDeveloperExists(string email)
            => _context.Developers.AnyAsync(u => u.Email == email);

        public async Task<bool> SaveDeveloperAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UpdateDeveloper(Guid id, UpdateUserDTO userDTO)
        {
            var dev = await _context.Developers.Where(e => e.Id == id).FirstOrDefaultAsync();

            dev.FirstName = userDTO.FirstName;
            dev.LastName = userDTO.LastName;
            _context.Developers.Update(dev);
            return await SaveDeveloperAsync();
        }

        public async Task<bool> AddDeveloperForProject(Guid id, List<Guid> devId)
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

            var isDevExist = devId.All(devId => _context.Developers.Any(x => x.Id == devId));

            if (!isDevExist)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Developers not found",
                    Detail = "Developers with such ids not found"
                };
            }

            var projDev = devId.Select(pd => new ProjectDeveloper
            {
                ProjectID = id,
                DeveloperID = pd
            }).ToList();

            _context.ProjectDevelopers.AddRange(projDev); 
            
            return await SaveDeveloperAsync();
        }

        public async Task<bool> RemoveDeveloperFromProject(Guid id, List<Guid> devId)
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

            var isDevExist = devId.All(devId => _context.Developers.Any(x => x.Id == devId));

            if (!isDevExist)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Developers not found",
                    Detail = "Developers with such ids not found"
                };
            }

            var projDev = devId.Select(pd => new ProjectDeveloper
            {
                ProjectID = id,
                DeveloperID = pd
            }).ToList();

            _context.ProjectDevelopers.RemoveRange(projDev);

            return await SaveDeveloperAsync();
        }
    }
}
