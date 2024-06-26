﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectCollaborationPlatform.DAL.Data.Models;

namespace ProjectCollaborationPlatform.DAL.Data.DataAccess.Configurations
{
    internal class FunctionalityBlockConfiguration : IEntityTypeConfiguration<FunctionalityBlock>
    {
        public void Configure(EntityTypeBuilder<FunctionalityBlock> builder)
        {
            builder
                .HasKey(t => t.Id);
        }
    }
}
