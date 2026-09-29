using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FinancialBox.Models;
public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    protected override void OnModelCreating(ModelBuilder builder)
{
    base.OnModelCreating(builder);

    builder.Entity<Box>()
        .Property(b => b.Balance)
        .HasPrecision(18, 2);

    builder.Entity<Box>()
        .Property(b => b.TargetAmount)
        .HasPrecision(18, 2);

    builder.Entity<Transaction>()
        .Property(t => t.Amount)
        .HasPrecision(18, 2);

    builder.Entity<Transfer>()
        .Property(t => t.Amount)
        .HasPrecision(18, 2);

    builder.Entity<Transfer>()
        .HasOne(t => t.FromBox)
        .WithMany()
        .HasForeignKey(t => t.FromBoxId)
        .OnDelete(DeleteBehavior.Restrict);
    
    builder.Entity<Transfer>()
        .HasOne(t => t.ToBox)
        .WithMany()
        .HasForeignKey(t => t.ToBoxId)
        .OnDelete(DeleteBehavior.Restrict);
}
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Box> FinancialBoxes { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Transfer> Transfers { get; set; }
}