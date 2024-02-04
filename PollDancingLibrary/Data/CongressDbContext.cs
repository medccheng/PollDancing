using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PollDancingLibrary.Models;

namespace PollDancingLibrary.Data
{
    public class CongressDbContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<Term> Terms { get; set; }

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
            //modelBuilder.Entity<MemberTerm>()
            //    .HasKey(mt => new { mt.MemberId, mt.TermId });

            //modelBuilder.Entity<MemberTerm>()
            //    .HasOne(mt => mt.Member)
            //    .WithMany(m => m.MemberTerms)
            //    .HasForeignKey(mt => mt.MemberId);

            //modelBuilder.Entity<MemberTerm>()
            //    .HasOne(mt => mt.Term)
            //    .WithMany(t => t.MemberTerms)
            //    .HasForeignKey(mt => mt.TermId);

            // Or, for the simplified approach (EF Core 5.0+), configure the many-to-many directly
            // This part is optional if EF's conventions are enough for your model
        }
    }
}
