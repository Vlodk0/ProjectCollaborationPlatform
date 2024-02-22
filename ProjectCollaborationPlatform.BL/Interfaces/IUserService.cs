

using ProjectCollaborationPlatform.DAL.Data.Models;

namespace ProjectCollaborationPlatform.BL.Interfaces
{
    public interface IUserService : IBaseService<User>
    {
        Task<List<User>> GetAllUsers();

    }
}
