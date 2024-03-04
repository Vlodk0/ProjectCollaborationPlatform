

using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.Domain.DTOs;

namespace ProjectCollaborationPlatform.BL.Interfaces
{
    public interface IBoardService
    {
        Task<Board> GetBoardByName(string name, CancellationToken token);
        Task<Board> GetBoardById(Guid id, CancellationToken token);
        Task<bool> CreateBoard(BoardDTO boardDTO);
        Task<bool> UpdateBoard(BoardDTO boardDTO);
        Task<bool> DeleteBoard(Guid id);
        Task<bool> SaveBoardAsync();
    }
}
