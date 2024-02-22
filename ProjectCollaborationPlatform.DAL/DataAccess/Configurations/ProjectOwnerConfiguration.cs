using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectCollaborationPlatform.DAL.Data.Models;


namespace ProjectCollaborationPlatform.DAL.Data.DataAccess.Configurations
{
    public class ProjectOwnerConfiguration : IEntityTypeConfiguration<ProjectOwner>
    {
        public void Configure(EntityTypeBuilder<ProjectOwner> builder)
        {
            builder
                .HasMany(p => p.Projects)
                .WithOne(po => po.ProjectOwner);
            builder
                .HasKey(p => p.Id);
        }
    }
}
