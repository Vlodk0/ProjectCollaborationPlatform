using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.DAL.Data.DataAccess;
using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.Domain.DTOs;
using ProjectCollaborationPlatform.Domain.Enums;
using ProjectCollaborationPlatform.Domain.Helpers;

namespace ProjectCollaborationPlatform.BL.Services
{
    public class FunctionalityBlockService : IFunctionalityBlockService
    {
        readonly ProjectPlatformContext _context;

        public FunctionalityBlockService(ProjectPlatformContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateFunctionalityBlock(FunctionalityBlockDTO functionalityBlock, Guid boardId)
        {
            var funcBlock = new FunctionalityBlock
            {
                Id = functionalityBlock.Id,
                Task = functionalityBlock.Task,
                Status = functionalityBlock.Status,
                BoardID = boardId
            };
            _context.FunctionalityBlocks.Add(funcBlock);
            return await SaveFunctionalityBlockAsync();
        }

        public async Task<FunctionalityBlockDTO> GetFunctionalityBlockById(Guid id, CancellationToken token)
        {
            var funcBlock = await _context.FunctionalityBlocks.Where(i => i.Id == id).FirstOrDefaultAsync(token);

            if (funcBlock == null)
            {
                return null;
            }

            return new FunctionalityBlockDTO()
            {
                Id = funcBlock.Id,
                Task = funcBlock.Task,
                Status = funcBlock.Status
            };
        }

        public async Task<bool> DeleteFunctionalityBlock(Guid id)
        {
            var entity = await _context.FunctionalityBlocks.Where(fb => fb.Id == id).FirstOrDefaultAsync();//you've already checked if it exists
            if (entity == null)
            {
                return false;
            }

            _context.FunctionalityBlocks.Remove(entity);

            return await SaveFunctionalityBlockAsync();
        }

        public async Task<List<FunctionalityBlockDTO>> GetFunctionalityBlocksByBoardId(Guid boardId, CancellationToken token)
        {
            var funcBlocks = await _context.FunctionalityBlocks
                .Where(fb => fb.BoardID == boardId) 
                .Select(fb => new FunctionalityBlockDTO
                {
                    Id = fb.Id,
                    Task = fb.Task,
                    Status = fb.Status
                })
                .ToListAsync(token);

            return funcBlocks;
        }

        public async Task<bool> UpdateFunctionalityBlockStatus(Guid id, StatusEnum status)
        {
            var funckBlockToUpdate = await _context.FunctionalityBlocks.Where(fb => fb.Id == id).FirstOrDefaultAsync();

            if (funckBlockToUpdate == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "FunctionalityBlock not found",
                    Detail = "FunctionalityBlock with such id doesn't exist"
                };
            }

            funckBlockToUpdate.Status = status;
            _context.FunctionalityBlocks.Update(funckBlockToUpdate);
            return await SaveFunctionalityBlockAsync();
        }

        public async Task<bool> SaveFunctionalityBlockAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UpdateFunctionalityBlock(Guid id, string task)
        {
            var funckBlock = await _context.FunctionalityBlocks.Where(n => n.Id == id).FirstOrDefaultAsync();

            funckBlock.Task = task;
            _context.FunctionalityBlocks.Update(funckBlock);
            return await SaveFunctionalityBlockAsync();
        }
    }
}
