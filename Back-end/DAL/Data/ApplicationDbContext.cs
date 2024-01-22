using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<LoanHistory> LoanHistory { get; set; }
        public DbSet<PasswordResetCode> PasswordResetCode { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Documents)
                .WithOne(d => d.Employee)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.LoanHistory)
                .WithOne(d => d.Employee)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Product>()
                .HasMany(e => e.LoanHistory)
                .WithOne(d => d.Product)
                .OnDelete(DeleteBehavior.Cascade);
    
            base.OnModelCreating(modelBuilder);
        }
    }
}
