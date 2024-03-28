using Microsoft.EntityFrameworkCore;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.DAL.Data.DataAccess;
using ProjectCollaborationPlatform.Domain.DTOs;

namespace ProjectCollaborationPlatform.BL.Services
{
    public class TaskService : ITaskService
    {
        readonly ProjectPlatformContext _context;

        public TaskService(ProjectPlatformContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateTask(TaskDTO taskDto)
        {
            var tsk = new DAL.Data.Models.Task
            {
                Description = taskDto.Description,
            };
            _context.Tasks.Add(tsk);
            return await SaveTaskAsync();
        }

        public async Task<bool> DeleteTask(Guid id)
        {
            var entity = await _context.Tasks.Where(t => t.Id == id).FirstOrDefaultAsync();
            if (entity == null)
            {
                return false;
            }

            _context.Tasks.Remove(entity);

            return await SaveTaskAsync();
        }

        public async Task<TaskDTO> GetTaskById(Guid id, CancellationToken token)
        {
            var task = await _context.Tasks.Where(t => t.Id == id).FirstOrDefaultAsync(token);

            if(task == null)
            {
                return null;
            }

            return new TaskDTO()
            {
                Description = task.Description
            };
        }

        public async Task<bool> SaveTaskAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UpdateTask(Guid id, string description)
        {
            var task = await _context.Tasks.Where(n => n.Id == id).FirstOrDefaultAsync();

            task.Description = description;
            return await SaveTaskAsync();
        }
    }
}
