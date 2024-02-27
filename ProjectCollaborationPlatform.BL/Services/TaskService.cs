using Microsoft.EntityFrameworkCore;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.DAL.Data.DataAccess;

namespace ProjectCollaborationPlatform.BL.Services
{
    public class TaskService : ITaskService
    {
        readonly ProjectPlatformContext _context;

        public TaskService(ProjectPlatformContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateTask(DAL.Data.Models.Task task)
        {
            var tsk = new DAL.Data.Models.Task
            {
                Descripion = task.Descripion,
            };
            _context.Set<DAL.Data.Models.Task>().Add(tsk);
            return await SaveTaskAsync();
        }

        public async Task<bool> DeleteTask(Guid id)
        {
            var entity = await _context.Set<DAL.Data.Models.Task>().FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            _context.Set<DAL.Data.Models.Task>().Remove(entity);

            return await SaveTaskAsync();
        }

        public async Task<DAL.Data.Models.Task> GetTaskById(Guid id)
        {
            return await _context.Set<DAL.Data.Models.Task>().FindAsync(id);
        }

        public async Task<DAL.Data.Models.Task> GetTaskByName(string name)
        {
            return await _context.Set<DAL.Data.Models.Task>().FindAsync(name);
        }

        public async Task<bool> SaveTaskAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateTask(DAL.Data.Models.Task task)
        {
            var tsk = await _context.Tasks.Where(n => n.Descripion == task.Descripion).FirstOrDefaultAsync();
            tsk = new DAL.Data.Models.Task()
            {
               Descripion = task.Descripion,
            };
            _context.Tasks.Update(tsk);
            return await SaveTaskAsync();
        }
    }
}
