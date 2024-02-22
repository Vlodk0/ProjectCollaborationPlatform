using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectCollaborationPlatform.DAL.Data.Models;

namespace ProjectCollaborationPlatform.DAL.Data.DataAccess.Configurations
{
    public class DeveloperTechnologyConfiguration : IEntityTypeConfiguration<DeveloperTechnology>
    {
        public void Configure(EntityTypeBuilder<DeveloperTechnology> builder)
        {
            builder
                .HasKey(td => new { td.DeveloperID, td.TechnologyID });
            builder
                .HasOne(td => td.Developer)
                .WithMany(t => t.DeveloperTechnologies)
                .HasForeignKey(td => td.DeveloperID);
            builder
                .HasOne(td => td.Technology)
                .WithMany(t => t.DeveloperTechnologies)
                .HasForeignKey(td => td.TechnologyID);
        }
    }
}
