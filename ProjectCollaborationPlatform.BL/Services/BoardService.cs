using Microsoft.EntityFrameworkCore;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.DAL.Data.DataAccess;
using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.Domain.DTOs;

namespace ProjectCollaborationPlatform.BL.Services
{
    public class BoardService : IBoardService
    {
        readonly ProjectPlatformContext _context;

        public BoardService(ProjectPlatformContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateBoard(BoardDTO boardDto)
        {
            var board = new Board
            {
                Name = boardDto.Name,
            };
            _context.Set<Board>().Add(board);
            return await SaveAsync();
        }

        public async Task<bool> DeleteBoard(Guid id)
        {

            var entity = await _context.Set<Board>().FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            _context.Set<Board>().Remove(entity);

            return await SaveAsync();
        }

        public async Task<BoardDTO> GetBoardByName(string name)
        {
            var board = await _context.Set<Board>().FindAsync(name);
            var boardDto = new BoardDTO
            {
                Name = board.Name,
            };
            return boardDto;
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateBoard(Board board)
        {
            _context.Entry(board).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return await SaveAsync();
        }
    }
}
