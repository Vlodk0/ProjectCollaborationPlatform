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
                Name = userDTO.Name,
                Email = userDTO.Email,
                Password = userDTO.Password,
                IsDeleted = false,
            };
            _context.Set<User>().Add(user);
            return await SaveUserAsync();
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            var user = await GetUserById(id);
            if (user == null)
            {
                return false;
            }
            var deletedUser = new User()
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Password = user.Password,
                IsDeleted = true,
                RoleName = user.RoleName,
            };

            _context.Set<User>().Update(deletedUser);

            return await SaveUserAsync();
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Set<User>().ToListAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Set<User>().FindAsync(email);
        }

        public async Task<User> GetUserById(Guid id)
        {
            return await _context.Set<User>().FindAsync(id);

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
                Name = userDTO.Name,
                Email = userDTO.Email,
                Password = userDTO.Password,
            };
            _context.Users.Update(user);
            return await SaveUserAsync();
        }
    }
}
