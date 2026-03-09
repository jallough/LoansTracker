
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.DataContext
{
    public class LoansDbContext : DbContext
    {
        public DbSet<Person> Persons => Set<Person>();
        public DbSet<Loan> Loans => Set<Loan>();

        public LoansDbContext(DbContextOptions<LoansDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
           
            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Id).ValueGeneratedOnAdd();
                entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
                entity.Property(s => s.Surname).IsRequired().HasMaxLength(100);
                entity.HasIndex(s => new { s.Name, s.Surname }).IsUnique();
            });
            modelBuilder.Entity<Loan>(entity =>
            {
                entity.HasKey(l => l.Id);
                entity.Property(l => l.Id).ValueGeneratedOnAdd();
                entity.HasOne(l => l.Person).WithMany().HasForeignKey(l => l.PersonId);
                entity.Property(l => l.Date).IsRequired();
            });
        }
    }
}
