using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.DAL.Models;

namespace ProjectCollaborationPlatform.DAL.Data.DataAccess.Configurations
{
    public class DeveloperConfiguration : IEntityTypeConfiguration<Developer>
    {
        public void Configure(EntityTypeBuilder<Developer> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .HasOne(b => b.PhotoFile)
                .WithOne(p => p.Developer)
                .HasForeignKey<Developer>(pr => pr.PhotoFileId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
