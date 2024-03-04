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
            return await SaveBoardAsync();
        }

        public async Task<bool> DeleteBoard(Guid id)
        {

            var entity = await _context.Set<Board>().FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            _context.Set<Board>().Remove(entity);

            return await SaveBoardAsync();
        }

        public async Task<Board> GetBoardById(Guid id, CancellationToken token)
        {
            return await _context.Set<Board>().FindAsync(id, token);
        }

        public async Task<Board> GetBoardByName(string name, CancellationToken token)
        {
            return await _context.Set<Board>().FindAsync(name, token);
        }

        public async Task<bool> SaveBoardAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateBoard(BoardDTO boardDTO)
        {
            var board = await _context.Boards.Where(n => n.Name == boardDTO.Name).FirstOrDefaultAsync();
            board = new Board()
            {
                Name = boardDTO.Name,
            };
            _context.Boards.Update(board);
            return await SaveBoardAsync();
        }
    }
}
