

using ProjectCollaborationPlatform.DAL.Data.Models;

namespace ProjectCollaborationPlatform.WebAPI.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAll();
        Task<User> Get(Guid id);
        Task<User> Add(User user);
        Task<User> Update(User user);
        Task<User> Delete(int id);
        Task<bool> SaveAsync();
        //Task<bool> isUserExist(string email, string password);
    }
}
