using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_FootballBetting.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P03_FootballBetting.Data.Configuration
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasOne(x => x.PrimaryKitColor)
                                      .WithMany(x => x.PrimaryKitTeams)
                                      .HasForeignKey(x => x.PrimaryKitColorId)
                                      .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SecondaryKitColor)
                                       .WithMany(x => x.SecondaryKitTeams)
                                       .HasForeignKey(x => x.SecondaryKitColorId)
                                       .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
