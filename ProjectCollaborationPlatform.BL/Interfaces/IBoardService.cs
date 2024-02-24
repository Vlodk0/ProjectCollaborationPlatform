

using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.Domain.DTOs;

namespace ProjectCollaborationPlatform.BL.Interfaces
{
    public interface IBoardService
    {
        Task<Board> GetBoardByName(string name);
        Task<Board> GetBoardById(Guid id);
        Task<bool> CreateBoard(BoardDTO boardDTO);
        Task<bool> UpdateBoard(BoardDTO boardDTO);
        Task<bool> DeleteBoard(Guid id);
        Task<bool> SaveBoardAsync();
    }
}
