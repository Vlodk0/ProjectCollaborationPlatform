

using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.Domain.DTOs;

namespace ProjectCollaborationPlatform.BL.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetUserById(Guid id, CancellationToken token);
        Task<UserDTO> GetUserByEmail(string email, CancellationToken token);
        Task<bool> AddUser(UserDTO user);
        Task<bool> UpdateUser(UserDTO userDTO);
        Task<bool> DeleteUser(Guid id);
        Task<bool> SaveUserAsync();
        Task<bool> IsUserExists(string email);

    }
}
