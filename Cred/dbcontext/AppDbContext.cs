using Microsoft.EntityFrameworkCore;
using Cred.Models;

namespace Cred.dbcontex
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<PreviousEmployment> PreviousEmployments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.EmployeeNo).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.PlaceOfBirth).HasMaxLength(100);
            });

            modelBuilder.Entity<PreviousEmployment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.JobTitle).IsRequired().HasMaxLength(100);

                entity.HasOne(e => e.Employee)
                      .WithMany(e => e.PreviousEmployments)
                      .HasForeignKey(e => e.EmployeeId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
