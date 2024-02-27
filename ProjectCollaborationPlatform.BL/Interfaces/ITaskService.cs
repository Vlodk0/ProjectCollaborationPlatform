

namespace ProjectCollaborationPlatform.BL.Interfaces
{
    public interface ITaskService
    {
        Task<DAL.Data.Models.Task> GetTaskByName(string name);
        Task<DAL.Data.Models.Task> GetTaskById(Guid id);
        Task<bool> CreateTask(DAL.Data.Models.Task task);
        Task<bool> UpdateTask(DAL.Data.Models.Task task);
        Task<bool> DeleteTask(Guid id);
        Task<bool> SaveTaskAsync();
    }
}
