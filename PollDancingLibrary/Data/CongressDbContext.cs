using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PollDancingLibrary.Models;
using Action = PollDancingLibrary.Models.Action;

namespace PollDancingLibrary.Data
{
    public class CongressDbContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<Term> Terms { get; set; }

        public DbSet<AddressInformation> AddressInformations { get; set; }

        public DbSet<Depiction> Depictions { get; set; }

        public DbSet<Action> Actions { get; set; }

        public DbSet<Legislation> Legislations { get; set; }

        public DbSet<SponsoredLegislation> SponsoredLegislations { get; set; }

        public DbSet<CosponsoredLegislation> CosponsoredLegislations { get; set; }
        public CongressDbContext(DbContextOptions<CongressDbContext> options)
        : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //// Explicit configuration for many-to-many relationship if using the explicit join entity approach
            modelBuilder.Entity<SponsoredLegislation>()
                .HasKey(mt => new { mt.MemberId, mt.LegislationId });

            modelBuilder.Entity<SponsoredLegislation>()
                .HasOne(mt => mt.Member)
                .WithMany(m => m.SponsoredLegislations)
                .HasForeignKey(mt => mt.MemberId);

            modelBuilder.Entity<SponsoredLegislation>()
                .HasOne(mt => mt.Legislation)
                .WithMany(t => t.SponsoredLegislations)
                .HasForeignKey(mt => mt.LegislationId);

            modelBuilder.Entity<CosponsoredLegislation>()
                .HasKey(mt => new { mt.MemberId, mt.LegislationId });

            modelBuilder.Entity<CosponsoredLegislation>()
                .HasOne(mt => mt.Member)
                .WithMany(m => m.CosponsoredLegislations)
                .HasForeignKey(mt => mt.MemberId);

            modelBuilder.Entity<CosponsoredLegislation>()
                .HasOne(mt => mt.Legislation)
                .WithMany(t => t.CosponsoredLegislations)
                .HasForeignKey(mt => mt.LegislationId);


            // Or, for the simplified approach (EF Core 5.0+), configure the many-to-many directly
            // This part is optional if EF's conventions are enough for your model
        }
    }
}
