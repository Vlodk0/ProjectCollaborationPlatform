using Microsoft.EntityFrameworkCore;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.DAL.Data.DataAccess;
using ProjectCollaborationPlatform.DAL.Data.Models;

namespace ProjectCollaborationPlatform.BL.Services
{
    public class FunctionalityBlockService : IFunctionalityBlockService
    {
        readonly ProjectPlatformContext _context;

        public FunctionalityBlockService(ProjectPlatformContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateFunctionalityBlock(FunctionalityBlock functionalityBlock)
        {
            var funcBlock = new FunctionalityBlock
            {
                Name = functionalityBlock.Name,
            };
            _context.Set<FunctionalityBlock>().Add(funcBlock);
            return await SaveFunctionalityBlockAsync();
        }

        public async Task<bool> DeleteFunctionalityBlock(Guid id)
        {
            var entity = await _context.Set<FunctionalityBlock>().FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            _context.Set<FunctionalityBlock>().Remove(entity);

            return await SaveFunctionalityBlockAsync();
        }

        public async Task<FunctionalityBlock> GetFunctionalityBlockById(Guid id, CancellationToken token)
        {
            return await _context.Set<FunctionalityBlock>().FindAsync(id, token);
        }

        public async Task<FunctionalityBlock> GetFunctionalityBlockByName(string name, CancellationToken token)
        {
            return await _context.Set<FunctionalityBlock>().FindAsync(name, token);
        }

        public async Task<bool> SaveFunctionalityBlockAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateFunctionalityBlock(FunctionalityBlock functionalityBlock)
        {
            var funckBlock = await _context.FunctionalityBlocks.Where(n => n.Name == functionalityBlock.Name).FirstOrDefaultAsync();
            funckBlock = new FunctionalityBlock()
            {
                Name = functionalityBlock.Name,
            };
            _context.FunctionalityBlocks.Update(funckBlock);
            return await SaveFunctionalityBlockAsync();
        }
    }
}
