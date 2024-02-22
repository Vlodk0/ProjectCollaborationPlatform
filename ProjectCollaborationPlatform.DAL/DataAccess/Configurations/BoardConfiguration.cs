using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectCollaborationPlatform.DAL.Data.Models;


namespace ProjectCollaborationPlatform.DAL.Data.DataAccess.Configurations
{
    public class BoardConfiguration : IEntityTypeConfiguration<Board>
    {
        public void Configure(EntityTypeBuilder<Board> builder)
        {
            builder
                .HasMany(f => f.FunctionalityBlocksID)
                .WithOne(b => b.Board);
            builder
                .HasKey(i => i.Id);
        }
    }
}
