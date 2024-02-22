using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectCollaborationPlatform.DAL.Data.Models;


namespace ProjectCollaborationPlatform.DAL.Data.DataAccess.Configurations
{
    public class ProjectDetailConfiguration : IEntityTypeConfiguration<ProjectDetail>
    {
        public void Configure(EntityTypeBuilder<ProjectDetail> builder)
        {
            builder
                .HasOne(p => p.Project)
                .WithOne(pr => pr.ProjectDetail)
                .HasForeignKey<Project>(p => p.ProjectDetailID);
            builder
                .HasKey(i => i.Id);
                
        }
    }
}
