using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.DAL.Models;


namespace ProjectCollaborationPlatform.DAL.Data.DataAccess.Configurations
{
    public class ProjectOwnerConfiguration : IEntityTypeConfiguration<ProjectOwner>
    {
        public void Configure(EntityTypeBuilder<ProjectOwner> builder)
        {
            builder
                .HasMany(p => p.Projects)
                .WithOne(po => po.ProjectOwner)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasKey(p => p.Id);

            builder
                .HasOne(b => b.PhotoFile)
                .WithOne(p => p.ProjectOwner)
                .HasForeignKey<ProjectOwner>(pr => pr.PhotoFileId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
