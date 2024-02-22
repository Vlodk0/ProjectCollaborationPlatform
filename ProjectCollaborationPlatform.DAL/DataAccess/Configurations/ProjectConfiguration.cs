using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectCollaborationPlatform.DAL.Data.Models;

namespace ProjectCollaborationPlatform.DAL.Data.DataAccess.Configurations
{
    public class ProjectConfiguration :  IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder
                .HasOne(b => b.Board)
                .WithOne(p => p.Project)
                .HasForeignKey<Board>(pr => pr.ProjectID);
            builder
                .HasOne(pd => pd.ProjectDetail)
                .WithOne(p => p.Project)
                .HasForeignKey<ProjectDetail>(pr => pr.ProjectID);
            builder
                .HasKey(i => i.Id);
        }
    }
}

