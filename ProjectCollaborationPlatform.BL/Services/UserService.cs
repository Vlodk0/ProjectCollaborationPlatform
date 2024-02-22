using ProjectCollaborationPlatform.DAL.Data.DataAccess;
using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.BL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ProjectCollaborationPlatform.BL.Services
{
    public class UserService :  BaseService<User>, IUserService
    {
        readonly ProjectPlatformContext _context;
        public UserService(ProjectPlatformContext contextUser) : base(contextUser)
        {
            _context = contextUser;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Set<User>().ToListAsync();
        }

    }
}
