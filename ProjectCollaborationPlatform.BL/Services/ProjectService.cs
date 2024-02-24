using Microsoft.EntityFrameworkCore;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.DAL.Data.DataAccess;
using ProjectCollaborationPlatform.DAL.Data.Models;
using System.Xml.Linq;

namespace ProjectCollaborationPlatform.BL.Services
{
    public class ProjectService : IProjectService
    {
        readonly ProjectPlatformContext _context;

        public ProjectService(ProjectPlatformContext context)
        {
            _context = context;
        }

        public async Task<bool> AddProject(Project project)
        {
            _context.Set<Project>().Add(project);
            return await SaveProjectAsync();
        }

        public async Task<bool> DeleteProjectById(Guid id)
        {
            var entity = await _context.Set<Project>().FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            _context.Set<Project>().Remove(entity);

            return await SaveProjectAsync();
        }

        public async Task<bool> DeleteProjectByName(string name)
        {
            var entity = await _context.Set<Project>().FindAsync(name);
            if (entity == null)
            {
                return false;
            }

            _context.Set<Project>().Remove(entity);

            return await SaveProjectAsync();
        }

        public async Task<List<Project>> GetAllProjects()
        {
            return await _context.Set<Project>().ToListAsync();
        }

        public async Task<Project> GetProjectById(Guid id)
        {
            return await _context.Set<Project>().FindAsync(id);
        }

        public async Task<Project> GetProjectByName(string name)
        {
            return await _context.Set<Project>().FindAsync(name);
        }

        public async Task<bool> SaveProjectAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateProject(Project project)
        {
            _context.Entry(project).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return await SaveProjectAsync();
        }
    }
}
