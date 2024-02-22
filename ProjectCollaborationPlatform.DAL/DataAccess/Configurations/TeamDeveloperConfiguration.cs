using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectCollaborationPlatform.DAL.Data.Models;

namespace ProjectCollaborationPlatform.DAL.Data.DataAccess.Configurations
{
    public class TeamDeveloperConfiguration : IEntityTypeConfiguration<TeamDeveloper>
    {
        public void Configure(EntityTypeBuilder<TeamDeveloper> builder)
        {
            builder
                .HasKey(td => new {td.DeveloperID, td.TeamID});
            builder
                .HasOne(td => td.Developer)
                .WithMany(t => t.TeamDevelopers)
                .HasForeignKey(td => td.DeveloperID);
            builder
                .HasOne(td => td.Team)
                .WithMany(t => t.TeamDevelopers)
                .HasForeignKey(td => td.TeamID);

        }
    }
}
