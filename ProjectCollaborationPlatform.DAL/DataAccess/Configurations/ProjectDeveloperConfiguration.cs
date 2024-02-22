using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectCollaborationPlatform.DAL.Data.Models;

namespace ProjectCollaborationPlatform.DAL.Data.DataAccess.Configurations
{
    public class ProjectDeveloperConfiguration : IEntityTypeConfiguration<ProjectDeveloper>
    {
        public void Configure(EntityTypeBuilder<ProjectDeveloper> builder)
        {
            builder
                .HasKey(td => new { td.ProjectID, td.DeveloperID });
            builder
                .HasOne(td => td.Project)
                .WithMany(t => t.ProjectDevelopers)
                .HasForeignKey(td => td.ProjectID);
            builder
                .HasOne(td => td.Developer)
                .WithMany(t => t.ProjectDevelopers)
                .HasForeignKey(td => td.DeveloperID);
        }
    }
}
