using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.DAL.Models;

namespace ProjectCollaborationPlatform.DAL.DataAccess.Configurations
{
    public class PhotoFileConfiguration : IEntityTypeConfiguration<PhotoFile>
    {
        public void Configure(EntityTypeBuilder<PhotoFile> builder)
        {
            builder
                .HasOne(b => b.Developer)
                .WithOne(p => p.PhotoFile)
                .HasForeignKey<PhotoFile>(pr => pr.DeveloperId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(b => b.ProjectOwner)
                .WithOne(p => p.PhotoFile)
                .HasForeignKey<PhotoFile>(pr => pr.ProjectOwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasKey(b => b.Id);
        }
    }
}
