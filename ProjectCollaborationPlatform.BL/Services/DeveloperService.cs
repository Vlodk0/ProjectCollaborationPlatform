using ProjectCollaborationPlatform.DAL.Data.DataAccess;
using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.BL.Interfaces;
using Microsoft.EntityFrameworkCore;
using ProjectCollaborationPlatform.Domain.DTOs;

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
            var dev = await _context.Set<Developer>().FindAsync(id);
            if (dev == null)
            {
                return false;
            }
            var deletedDev = new Developer()
            {
                Id = dev.Id,
                FirstName = dev.FirstName,
                LastName = dev.LastName,
                Email = dev.Email,
                IsDeleted = true,
            };

            _context.Set<Developer>().Update(deletedDev);

            return await SaveDeveloperAsync();
        }

        public async Task<DeveloperDTO> GetDeveloperByEmail(string email, CancellationToken token)
        {
            var dev = await _context.Developers.Where(e => e.Email == email).FirstOrDefaultAsync(token);

            return new DeveloperDTO()
            {
                Id = dev.Id,
                Email = dev.Email,
                FirstName = dev.FirstName,
                LastName = dev.LastName,
            };
        }

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
            };

        }

        public Task<bool> IsDeveloperExists(string email)
            => _context.Developers.AnyAsync(u => u.Email == email);

        public async Task<bool> SaveDeveloperAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UpdateDeveloper(DeveloperDTO developerDTO)
        {
            var dev = await _context.Developers.Where(e => e.Email == developerDTO.Email).FirstOrDefaultAsync();
            dev = new Developer()
            {
                FirstName = developerDTO.FirstName,
                LastName = developerDTO.LastName,
                Id = developerDTO.Id,
                IsDeleted = false

            };
            _context.Developers.Update(dev);
            return await SaveDeveloperAsync();
        }
    }
}
