using Microsoft.EntityFrameworkCore;
using ProjectCollaborationPlatform.DAL.Data.DataAccess;
using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.WebAPI.Interfaces;

namespace ProjectCollaborationPlatform.DAL.Repositories
{
    public class UserService : IUserService
    {
        readonly ProjectPlatformContext _context;
        public UserService(ProjectPlatformContext context)
        {
            _context = context;
        }

        public async Task<User> Add(User user)
        {
            _context.Set<User>().Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> Delete(int id)
        {
            var entity = await _context.Set<User>().FindAsync(id);
            if (entity == null)
            {
                return entity;
            }

            _context.Set<User>().Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<User> Get(Guid id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _context.Set<User>().FindAsync(id);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<List<User>> GetAll()
        {
            return await _context.Set<User>().ToListAsync();
        }

        public async Task<User> Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        //public async Task<bool> isUserExist(string email, string password)
        //{
        //    return await context.T.AnyAsync(e => e.email == email && e.password == password);
        //}

    }
}
