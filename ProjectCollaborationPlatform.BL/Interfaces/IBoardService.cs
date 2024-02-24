

using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.Domain.DTOs;

namespace ProjectCollaborationPlatform.BL.Interfaces
{
    public interface IBoardService
    {
        Task<BoardDTO> GetBoardByName(string name);
        Task<bool> CreateBoard(BoardDTO board);
        Task<bool> UpdateBoard(Board board);
        Task<bool> DeleteBoard(Guid id);
        Task<bool> SaveAsync();
    }
}
