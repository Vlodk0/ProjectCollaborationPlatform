using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.DAL.Data.DataAccess;
using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.Domain.DTOs;
using ProjectCollaborationPlatform.Domain.Helpers;

namespace ProjectCollaborationPlatform.BL.Services
{
    public class BoardService : IBoardService
    {
        private readonly ProjectPlatformContext _context;

        public BoardService(ProjectPlatformContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateBoard(Guid id, BoardDTO boardDto)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Project Not Found",
                    Detail = "Project with the specified id was not found"
                };
            }

            var board = new Board()
            {
                Id = boardDto.Id,
                Name = boardDto.Name,
                ProjectID = project.Id
            };

            _context.Boards.Add(board);

            if (!await SaveBoardAsync())
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Title = "Server Error",
                    Detail = "Error occurred while creating project"
                };
            }
            return true;
        }


        public async Task<bool> DeleteBoard(string name, CancellationToken token)
        {
            var board = await _context.Boards.FirstOrDefaultAsync(i => i.Name == name, token);
            if (board == null)
            {
                throw new CustomApiException
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Board not found",
                    Detail = "Board with such name doesn't exist"
                };
            }

            _context.Boards.Remove(board);

            return await SaveBoardAsync();
        }

        public async Task<BoardDTO?> GetBoardByName(string name, CancellationToken token)
        {
            var board = await _context.Boards.Where(i => i.Name == name).FirstOrDefaultAsync(token);

            if (board == null)
            {
                return null;
            }

            //TODO: it's not obvious why you return only the name in the whole DTO
            //maybe it's all YOUR frontend needs, but please keep in mind that you are building an API which many frontends might use in future
            //so you have to build it like you don't know nothing about frontend
            return new BoardDTO
            {
                Name = board.Name,
            };
        }

        public async Task<bool> SaveBoardAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UpdateBoard(Guid id, string name)
        {
            var board = await _context.Boards.Where(n => n.ProjectID == id).FirstOrDefaultAsync();

            if (board is null)
            {
                //TODO: handle null
                throw new CustomApiException
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Board not found",
                    Detail = "Board with such name doesn't exist"
                };
            }
            
            board.Name = name;
            
            _context.Boards.Update(board);
            return await SaveBoardAsync();
        }
    }
}
