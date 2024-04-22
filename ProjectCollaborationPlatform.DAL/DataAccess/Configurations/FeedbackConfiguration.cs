using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectCollaborationPlatform.DAL.Models;

namespace ProjectCollaborationPlatform.DAL.DataAccess.Configurations
{
    public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder
                .HasOne(td => td.ProjectOwner)
                .WithMany(f => f.Feedbacks)
                .HasForeignKey(td => td.ProjectOwnerID)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(td => td.Developer)
                .WithMany(t => t.Feedbacks)
                .HasForeignKey(td => td.DeveloperId)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasKey(p => p.Id);
        }
    }
}
