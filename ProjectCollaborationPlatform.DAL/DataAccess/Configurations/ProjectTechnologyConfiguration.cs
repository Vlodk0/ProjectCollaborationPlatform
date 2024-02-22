using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectCollaborationPlatform.DAL.Data.Models;

namespace ProjectCollaborationPlatform.DAL.Data.DataAccess.Configurations
{
    public class ProjectTechnologyConfiguration : IEntityTypeConfiguration<ProjectTechnology>
    {
        public void Configure(EntityTypeBuilder<ProjectTechnology> builder)
        {
            builder
                .HasKey(td => new { td.ProjectID, td.TechnologyID });
            builder
                .HasOne(td => td.Project)
                .WithMany(t => t.ProjectTechnologies)
                .HasForeignKey(td => td.ProjectID);
            builder
                .HasOne(td => td.Technology)
                .WithMany(t => t.ProjectTechnologies)
                .HasForeignKey(td => td.TechnologyID);
        }
    }
}
