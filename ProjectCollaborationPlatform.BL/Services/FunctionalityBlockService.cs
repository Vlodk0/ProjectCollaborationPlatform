﻿using Microsoft.EntityFrameworkCore;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.DAL.Data.DataAccess;
using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.Domain.DTOs;

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
                Name = functionalityBlock.Name,
                BoardID = boardId
            };
            _context.FunctionalityBlocks.Add(funcBlock);
            return await SaveFunctionalityBlockAsync();
        }

        public async Task<bool> DeleteFunctionalityBlock(Guid id)
        {
            var entity = await _context.FunctionalityBlocks.Where(fb => fb.Id == id).FirstOrDefaultAsync();
            if (entity == null)
            {
                return false;
            }

            _context.FunctionalityBlocks.Remove(entity);

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
                Name = funcBlock.Name
            };
        }

        public async Task<FunctionalityBlockDTO> GetFunctionalityBlockByName(string name, CancellationToken token)
        {
            var funcBlock = await _context.FunctionalityBlocks.Where(i => i.Name == name).FirstOrDefaultAsync(token);

            if (funcBlock == null)
            {
                return null;
            }

            return new FunctionalityBlockDTO()
            {
                Name = funcBlock.Name
            };
        }

        public async Task<bool> SaveFunctionalityBlockAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UpdateFunctionalityBlock(string name)
        {
            var funckBlock = await _context.FunctionalityBlocks.Where(n => n.Name == name).FirstOrDefaultAsync();

            funckBlock.Name = name;
            _context.FunctionalityBlocks.Update(funckBlock);
            return await SaveFunctionalityBlockAsync();
        }
    }
}
