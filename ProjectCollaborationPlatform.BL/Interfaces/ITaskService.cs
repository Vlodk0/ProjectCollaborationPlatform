

using ProjectCollaborationPlatform.Domain.DTOs;

namespace ProjectCollaborationPlatform.BL.Interfaces
{
    public interface ITaskService
    {
        Task<TaskDTO> GetTaskById(Guid id, CancellationToken token);
        Task<bool> CreateTask(TaskDTO taskDto);
        Task<bool> UpdateTask(Guid id, string description);
        Task<bool> DeleteTask(Guid id);
        Task<bool> SaveTaskAsync();
    }
}
