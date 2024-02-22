using Microsoft.EntityFrameworkCore;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.DAL.Data.DataAccess;
using ProjectCollaborationPlatform.DAL.Data.Models;

namespace ProjectCollaborationPlatform.BL.Services
{
    public class ProjectService : BaseService<Project>, IProjectService
    {
        readonly ProjectPlatformContext _context;
        public ProjectService(ProjectPlatformContext projectContext) : base(projectContext)
        {
            _context = projectContext;
        }

        public async Task<List<Project>> GetAllProjects()
        {
            return await _context.Set<Project>().ToListAsync();
        }
    }
}
