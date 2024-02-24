using Microsoft.EntityFrameworkCore;
using ProjectCollaborationPlatform.DAL.Data.DataAccess.Configurations;
using ProjectCollaborationPlatform.DAL.Data.Models;

namespace ProjectCollaborationPlatform.DAL.Data.DataAccess
{
    public class ProjectPlatformContext : DbContext
    {
        public DbSet<Board> Boards { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<DeveloperTechnology> DeveloperTechnologies { get; set; }
        public DbSet<FunctionalityBlock> FunctionalityBlocks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectDetail> ProjectDetails { get; set; }
        public DbSet<ProjectDeveloper> ProjectDevelopers { get; set; }
        public DbSet<ProjectTechnology> ProjectTechnologies { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamDeveloper> TeamDevelopers { get; set; }
        public DbSet<Technology> Technologies { get; set; }
        public DbSet<User> Users { get; set; }

        public ProjectPlatformContext()
        {

        }

        public ProjectPlatformContext(DbContextOptions<ProjectPlatformContext> option) : base(option)
        {
            
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new BoardConfiguration()); 
            builder.ApplyConfiguration(new DeveloperTechnologyConfiguration()); 
            builder.ApplyConfiguration(new FunctionalityBlockConfiguration()); 
            builder.ApplyConfiguration(new ProjectConfiguration()); 
            builder.ApplyConfiguration(new ProjectDetailConfiguration()); 
            builder.ApplyConfiguration(new ProjectDeveloperConfiguration()); 
            builder.ApplyConfiguration(new ProjectTechnologyConfiguration()); 
            builder.ApplyConfiguration(new TeamDeveloperConfiguration()); 
            base.OnModelCreating(builder);
        }

    }
}
