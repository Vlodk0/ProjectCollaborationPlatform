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
                .HasKey(i => i.Id);
                
        }
    }
}
