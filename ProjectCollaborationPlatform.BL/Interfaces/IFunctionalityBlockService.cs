﻿using ProjectCollaborationPlatform.Domain.DTOs;

namespace ProjectCollaborationPlatform.BL.Interfaces
{
    public interface IFunctionalityBlockService
    {
        Task<FunctionalityBlockDTO> GetFunctionalityBlockById(Guid id, CancellationToken token);
        Task<bool> CreateFunctionalityBlock(FunctionalityBlockDTO functionalityBlockDto, Guid boardId);
        Task<bool> UpdateFunctionalityBlock(Guid id, string task);
        Task<bool> DeleteFunctionalityBlock(Guid id);
        Task<bool> SaveFunctionalityBlockAsync();
    }
}
