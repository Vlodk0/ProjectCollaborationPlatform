

using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.Domain.DTOs;

namespace ProjectCollaborationPlatform.BL.Interfaces
{
    public interface IBoardService
    {
        Task<BoardDTO> GetBoardByName(string name, CancellationToken token);
        Task<BoardDTO> GetBoardById(Guid id, CancellationToken token);
        Task<bool> CreateBoard(Guid id, BoardDTO boardDTO);
        Task<bool> UpdateBoard(Guid id, string name);
        Task<bool> DeleteBoard(string name, CancellationToken token);
        Task<bool> SaveBoardAsync();
    }
}
