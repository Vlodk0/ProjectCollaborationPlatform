using ProjectCollaborationPlatform.DAL.Data.DataAccess;
using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.BL.Interfaces;
using Microsoft.EntityFrameworkCore;
using ProjectCollaborationPlatform.Domain.DTOs;

namespace ProjectCollaborationPlatform.BL.Services
{
    public class UserService :  IUserService
    {
        readonly ProjectPlatformContext _context;

        public UserService(ProjectPlatformContext context)
        {
            _context = context;
        }

        public async Task<bool> AddUser(UserDTO userDTO)
        {
            var user = new User()
            {
                Email = userDTO.Email,
                RoleName = userDTO.RoleName,
                IsDeleted = false,
            };
            _context.Set<User>().Add(user);
            return await SaveUserAsync();
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            var user = await _context.Set<User>().FindAsync(id);
            if (user == null)
            {
                return false;
            }
            var deletedUser = new User()
            {
                Email = user.Email,
                IsDeleted = true,
                RoleName = user.RoleName,
            };

            _context.Set<User>().Update(deletedUser);

            return await SaveUserAsync();
        }

        public async Task<List<User>> GetAllUsers(CancellationToken token)
        {
            return await _context.Set<User>().ToListAsync(token);
        }

        public async Task<User> GetUserByEmail(string email, CancellationToken token)
        {
            return await _context.Users.Where(e => e.Email == email).FirstOrDefaultAsync(token);
        }

        public async Task<User> GetUserById(Guid id, CancellationToken token)
        {
            return await _context.Users.Where(i => i.Id == id).FirstOrDefaultAsync(token);

        }

        public async Task<bool> SaveUserAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateUser(UserDTO userDTO)
        {
            var user = await _context.Users.Where(e => e.Email == userDTO.Email).FirstOrDefaultAsync();
            user = new User()
            {
                Email = userDTO.Email,
            };
            _context.Users.Update(user);
            return await SaveUserAsync();
        }
    }
}
